// ReSharper disable once RedundantNullableDirective
#nullable enable  // Suppress warning CS8669

using System.Collections.Generic;

using Unity.Entities;

using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



[UpdateInGroup(typeof(HumanAI))]
[UpdateAfter(typeof(GoalSelector))]
public partial class ForageOnTile_TaskSelector : SystemBase
{
	private record PathInfo(
		IReadOnlyList<AxialPosition> Path,
		float Cost
	);

	private record TargetResourceInfo(
		AxialPosition Position,
		PathInfo PathInfo,
		Entity Entity
	);


	// private RectangularHexMap? _map;



	// public void InitForScene(RectangularHexMap map)
	// {
	// 	_map = map;
	// }



	protected override void OnUpdate()
	{
		// if (_map == null)
		// 	return;

		foreach (var (foragerPosition, path,
			         taskEnabled, forageTaskEnabled,
			         foragerEntity) in
		         SystemAPI.Query<
			         MapPosition,
			         DynamicBuffer<PathTile>,
			         EnabledRefRW<Task>, EnabledRefRW<ForageOnTile_Task>
			         >()
			         .WithAll<Forage_Goal>()
			         .WithDisabled<Task, ForageOnTile_Task>()
			         .WithEntityAccess())
		{
			TargetResourceInfo? target = null;

			// Select resource by distance
			foreach (var (resourcePosition, ripeBiomass, resourceEntity)
			         in SystemAPI.Query<MapPosition, RipeBiomass>().WithEntityAccess())
			{
				if (ripeBiomass.IsZero)
					continue;

				var pathInfo = CalculatePath(foragerPosition, resourcePosition);
				if (target == null || pathInfo.Cost < target.PathInfo.Cost) {
					target = new TargetResourceInfo(resourcePosition, pathInfo, resourceEntity);
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

		return new PathInfo(path, path.Length * 1f);
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
