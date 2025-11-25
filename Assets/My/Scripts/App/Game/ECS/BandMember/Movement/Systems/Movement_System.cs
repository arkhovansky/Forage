using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.BandMember.Movement.Rules;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Movement.Systems {



[UpdateInGroup(typeof(DomainSimulation))]
public partial struct Movement_System : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var rules = SystemAPI.GetComponent<Movement_Rules>(state.SystemHandle);

		float cellPhysicalInnerDiameter = SystemAPI.GetSingleton<PhysicalMapParameters>().TileInnerDiameter;

		foreach (var (walker,
			         mapPosition, intraCellMovement, path_,
			         movementActivity, movementActivityEnabled, activityEnabled)
		         in SystemAPI.Query<
			         RefRO<Walker>,
			         RefRW<MapPosition>, RefRW<IntraCellMovement>, DynamicBuffer<PathTile>,
			         MovementActivity, EnabledRefRW<MovementActivity>, EnabledRefRW<Activity>
		         >())
		{
			var path = path_;

			Assert.IsTrue(!(path.Length == 0 && intraCellMovement.ValueRO.IsAtCenter));

			var hoursDelta = SystemAPI.GetSingleton<GameTime.Components.GameTime>().DeltaHours;
			bool arrived;

			do {
				if (path.Length == 0) {  // Inside target cell
					Assert.IsTrue(intraCellMovement.ValueRO.IsBeforeCenter);
					Assert.IsTrue(mapPosition.ValueRO.Value == movementActivity.TargetPosition);

					rules.MoveInsideDestinationCell(ref intraCellMovement.ValueRW,
					                                ref hoursDelta,
					                                cellPhysicalInnerDiameter,
					                                walker.ValueRO);
				}
				else {  // Not reached target cell yet
					Assert.IsTrue(path[^1].Position == movementActivity.TargetPosition);

					bool advancedToNextCell =
						rules.MoveInsideTransitCell(ref intraCellMovement.ValueRW,
						                            ref hoursDelta,
						                            cellPhysicalInnerDiameter,
						                            walker.ValueRO,
						                            mapPosition.ValueRO.Value);

					if (advancedToNextCell) {
						mapPosition.ValueRW.Value = path[0].Position;
						path.RemoveAt(0);
					}
				}

				arrived = path.Length == 0 && intraCellMovement.ValueRO.IsAtCenter;
			}
			while (! (Mathf.Approximately(hoursDelta, 0) || arrived));


			// Possibly stop activity
			if (arrived) {
				activityEnabled.ValueRW = false;
				movementActivityEnabled.ValueRW = false;
			}
		}
	}
}



}
