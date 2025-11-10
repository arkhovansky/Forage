using System.Collections.Generic;

using Unity.Entities;

using Lib.Grid;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.AI.Rules;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI.Systems {



[UpdateInGroup(typeof(HumanAI))]
[UpdateAfter(typeof(GoalSelector))]
public partial class LeisureTaskPlanner : SystemBase
{
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
		var campPosition = SystemAPI.GetComponent<MapPosition>(campEntity).Value;

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
			var pathInfo = AI_Movement_Rules.CalculatePath(position, campPosition);

			SetPath(in path, pathInfo.Path);


			// Start task

			taskEnabled.ValueRW = true;
			leisureTaskEnabled.ValueRW = true;


			// Start activity

			SystemAPI.SetComponentEnabled<Activity>(entity, true);

			if (position != campPosition) {
				SystemAPI.SetComponent(entity, new MovementActivity(campPosition));
				SystemAPI.SetComponentEnabled<MovementActivity>(entity, true);
			}
			else {
				SystemAPI.SetComponentEnabled<LeisureActivity>(entity, true);
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
