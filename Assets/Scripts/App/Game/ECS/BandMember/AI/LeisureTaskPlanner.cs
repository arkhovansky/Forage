using System.Collections.Generic;

using Unity.Entities;

using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



[UpdateInGroup(typeof(HumanAI))]
[UpdateAfter(typeof(GoalSelector))]
public partial class LeisureTaskPlanner : SystemBase
{
	private record PathInfo(
		IReadOnlyList<AxialPosition> Path,
		float Cost
	);


	// private RectangularHexMap? _map;



	// public void InitForScene(RectangularHexMap map)
	// {
	// 	_map = map;
	// }



	protected override void OnCreate()
	{
		base.OnCreate();

		RequireForUpdate<Camp.Components.Camp>();
	}



	protected override void OnUpdate()
	{
		// if (_map == null)
		// 	return;

		var campEntity = SystemAPI.GetSingletonEntity<Camp.Components.Camp>();
		var campPosition = SystemAPI.GetComponent<MapPosition>(campEntity).Position;

		foreach (var (position,
			         path,
			         taskEnabled, leisureTaskEnabled,
			         entity)
		         in SystemAPI.Query<
			         MapPosition,
			         DynamicBuffer<PathTile>,
			         EnabledRefRW<Task>, EnabledRefRW<Leisure_Task>
			         >()
			         .WithAll<Leisure_Goal>()
			         .WithDisabled<Task, Leisure_Task>()
			         .WithEntityAccess())
		{
			var pathInfo = CalculatePath(position.Position, campPosition);

			SetPath(in path, pathInfo.Path);


			// Start task

			taskEnabled.ValueRW = true;
			leisureTaskEnabled.ValueRW = true;


			// Start activity

			SystemAPI.SetComponentEnabled<Activity>(entity, true);

			if (position.Position != campPosition) {
				SystemAPI.SetComponent(entity, new MovementActivity(campPosition));
				SystemAPI.SetComponentEnabled<MovementActivity>(entity, true);
			}
			else {
				SystemAPI.SetComponentEnabled<LeisureActivity>(entity, true);
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
