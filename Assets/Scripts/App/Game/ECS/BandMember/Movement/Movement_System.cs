using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Movement {


[UpdateInGroup(typeof(DomainSimulation))]
public partial struct Movement_System : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		const float movementCost = 2f;

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
			var speed = walker.ValueRO.BaseSpeed_KmPerH / movementCost;
			bool arrived;

			do {
				if (path.Length == 0) {  // Inside target cell
					Assert.IsTrue(intraCellMovement.ValueRO.IsBeforeCenter);
					Assert.IsTrue(mapPosition.ValueRO.Value == movementActivity.TargetPosition);

					var distanceToCellCenter = intraCellMovement.ValueRO.DistanceToCenter * cellPhysicalInnerDiameter;
					var hoursToCellCenter = distanceToCellCenter / speed;

					if (hoursDelta > hoursToCellCenter) {  // Reached cell center and beyond
						intraCellMovement.ValueRW.SetAtCenter();

						hoursDelta -= hoursToCellCenter;
					}
					else {  // Not reached cell center
						var distance = hoursDelta * speed;
						intraCellMovement.ValueRW.Advance(distance / cellPhysicalInnerDiameter);

						hoursDelta = 0;
					}
				}
				else {  // Not reached target cell yet
					Assert.IsTrue(path[^1].Position == movementActivity.TargetPosition);

					var distanceToCellEdge = intraCellMovement.ValueRO.DistanceToFinalEdge * cellPhysicalInnerDiameter;
					var hoursToCellEdge = distanceToCellEdge / speed;

					if (hoursDelta > hoursToCellEdge) {  // Reached cell edge and beyond
						intraCellMovement.ValueRW.SetAtStartEdge(previousPosition: mapPosition.ValueRO.Value);
						mapPosition.ValueRW.Value = path[0].Position;
						path.RemoveAt(0);

						hoursDelta -= hoursToCellEdge;
					}
					else {  // Not reached cell edge
						var distance = hoursDelta * speed;
						intraCellMovement.ValueRW.Advance(distance / cellPhysicalInnerDiameter);

						hoursDelta = 0;
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
