// ReSharper disable once RedundantNullableDirective
#nullable enable  // Suppress warning CS8669

using System.Collections.Generic;

using Unity.Entities;

using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Gathering;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.Movement;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



[UpdateInGroup(typeof(HumanAI))]
[UpdateAfter(typeof(GoalSelector))]
public partial class ForageOnTile_TaskSelector : SystemBase
{
	private record PathInfo(
		IReadOnlyList<AxialPosition> Path,
		float TotalMovementCost
	);

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

			// Search in concentric rings
			for (uint radius = 0; ; radius++) {
				// Break if we already have a target more optimal than theoretically possible for the current ring
				if (target != null) {
					float minRingForageTime = GetMinForagingTime(radius, mapParams, walker, gatherer, foodConsumer);
					if (target.ForageTime < minRingForageTime)
						break;
				}

				var ringCells = map.GetRingCells(foragerPosition, radius);
				if (ringCells.Count == 0)
					break;

				foreach (var cell in ringCells) {
					var tileEntity = ecsMap.GetTileEntity(cell);
					var resourceEntity = SystemAPI.GetComponent<TilePlantResource>(tileEntity).ResourceEntity;

					if (resourceEntity == Entity.Null)
						continue;

					var ripeBiomass = SystemAPI.GetComponent<RipeBiomass>(resourceEntity);

					if (ripeBiomass.IsZero)
						continue;

					var pathInfo = CalculatePath(foragerPosition,
					                             SystemAPI.GetComponent<MapPosition>(resourceEntity));
					float forageTime = GetForagingTime(
						pathInfo, mapParams, walker, ripeBiomass, gatherer, foodConsumer);

					if (target == null || forageTime < target.ForageTime) {
						target = new TargetResourceInfo(cell, pathInfo, resourceEntity, forageTime);
					}
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



	private PathInfo CalculatePath(AxialPosition start, AxialPosition end)
	{
		var path = HexLayout.GetLinearPath(start, end);

		return new PathInfo(path, path.Length * Movement_System.MovementCost);
	}


	private float GetForagingTime(PathInfo pathInfo, PhysicalMapParameters physicalMapParams, Walker walker,
	                              RipeBiomass ripeBiomass, Gatherer gatherer, FoodConsumer foodConsumer)
	{
		float moveTime = Movement_System.GetMovementTime(
			pathInfo.TotalMovementCost, physicalMapParams.TileInnerDiameter, walker.BaseSpeed_KmPerH);
		float gatherTime = Gathering_System.GetGatheringTime(
			foodConsumer.EnergyNeededPerDay,
			ripeBiomass.Value, physicalMapParams.CellArea, gatherer.GatheringSpeed);

		return GetForagingTime(moveTime, gatherTime);
	}


	private float GetMinForagingTime(uint pathLength, PhysicalMapParameters physicalMapParams, Walker walker,
	                                 Gatherer gatherer, FoodConsumer foodConsumer)
	{
		float moveTime = Movement_System.GetMinMovementTime(
			pathLength, physicalMapParams.TileInnerDiameter, walker.BaseSpeed_KmPerH);
		float gatherTime = Gathering_System.GetMinGatheringTime(
			foodConsumer.EnergyNeededPerDay, gatherer.GatheringSpeed);

		return GetForagingTime(moveTime, gatherTime);
	}


	private float GetForagingTime(float moveTime, float gatherTime)
	{
		return moveTime * 2 + gatherTime;
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
