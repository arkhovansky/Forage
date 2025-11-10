// ReSharper disable once RedundantNullableDirective
#nullable enable  // Suppress warning CS8669

using System.Collections.Generic;

using Unity.Entities;

using Lib.Grid;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.AI.Rules;
using App.Game.ECS.BandMember.Energy.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI.Systems {



[UpdateInGroup(typeof(HumanAI))]
[UpdateAfter(typeof(GoalSelector))]
public partial class ForageOnTile_TaskSelector : SystemBase
{
	private record TargetResourceInfo(
		AxialPosition Position,
		PathInfo PathInfo,
		Entity Entity,
		float ForageTime
	);



	protected override void OnUpdate()
	{
		var map = SystemAPI.GetSingleton<Map.Components.Singletons.Map>().Value;
		var tileIndexBuffer = SystemAPI.GetSingletonBuffer<MapTileEntity>(isReadOnly: true);
		var mapParams = SystemAPI.GetSingleton<PhysicalMapParameters>();
		var ecsMap = new EcsMap(map, tileIndexBuffer);

		foreach (var (foragerPosition, walker, gatherer, foodConsumer,
			         path,
			         taskEnabled, forageTaskEnabled,
			         foragerEntity)
		         in SystemAPI.Query<
			         MapPosition, Walker, Gatherer, FoodConsumer,
			         DynamicBuffer<PathTile>,
			         EnabledRefRW<Task>, EnabledRefRW<ForageOnTile_Task>
			         >()
			         .WithAll<Forage_Goal>()
			         .WithDisabled<Task, ForageOnTile_Task>()
			         .WithEntityAccess())
		{
			TargetResourceInfo? target = null;

			foreach (var (ripeBiomass, resourcePosition, resourceEntity)
			         in SystemAPI.Query<RipeBiomass, MapPosition>().WithEntityAccess())
			{
				if (ripeBiomass.IsZero)
					continue;

				// Skip if we already have a target more optimal than theoretically possible for the current resource
				if (target != null) {
					float minResourceForageTime = AI_Foraging_Rules.GetMinForagingTime(
						foragerPosition, resourcePosition, mapParams, walker, gatherer, foodConsumer);
					if (target.ForageTime < minResourceForageTime)
						continue;
				}

				var pathInfo = AI_Movement_Rules.CalculatePath(foragerPosition, resourcePosition);
				float forageTime = AI_Foraging_Rules.GetForagingTime(
					pathInfo, mapParams, walker, ripeBiomass, gatherer, foodConsumer);

				if (target == null || forageTime < target.ForageTime) {
					target = new TargetResourceInfo(resourcePosition, pathInfo, resourceEntity, forageTime);
				}
			}

			if (target == null)
				continue;

			SetPath(in path, target.PathInfo.Path);


			// Create task

			taskEnabled.ValueRW = true;

			SystemAPI.SetComponent(foragerEntity, new ForageOnTile_Task(target.Position, target.Entity));
			forageTaskEnabled.ValueRW = true;


			// Start activity

			SystemAPI.SetComponentEnabled<Activity>(foragerEntity, true);

			if (target.Position != foragerPosition) {
				SystemAPI.SetComponent(foragerEntity, new MovementActivity(target.Position));
				SystemAPI.SetComponentEnabled<MovementActivity>(foragerEntity, true);
			}
			else {
				SystemAPI.SetComponent(foragerEntity, new GatheringActivity(target.Entity));
				SystemAPI.SetComponentEnabled<GatheringActivity>(foragerEntity, true);
			}
		}
	}



	private void SetPath(in DynamicBuffer<PathTile> pathBuffer, IReadOnlyList<AxialPosition> pathPositions)
	{
		var pathBuffer_ = pathBuffer;
		pathBuffer_.Length = 0;
		foreach (var position in pathPositions)
			pathBuffer_.Add(new PathTile(position));
	}
}



}
