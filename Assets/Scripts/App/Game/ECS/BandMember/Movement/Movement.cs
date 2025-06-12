using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Movement {


[UpdateInGroup(typeof(DomainSimulation))]
public partial struct Movement : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		const float cellEdgeDiameter_Km = 1f;
		const float movementCost = 2f;

		foreach (var (walker,
			         tilePosition, intraCellMovement, path_,
			         movementActivity, movementActivityEnabled, activityEnabled)
		         in SystemAPI.Query<
			         RefRO<Walker>,
			         RefRW<TilePosition>, RefRW<IntraCellMovement>, DynamicBuffer<PathTile>,
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
					Assert.IsTrue(tilePosition.ValueRO.Position == movementActivity.TargetPosition);

					var distanceToCellCenter = intraCellMovement.ValueRO.DistanceToCenter * cellEdgeDiameter_Km;
					var hoursToCellCenter = distanceToCellCenter / speed;

					if (hoursDelta > hoursToCellCenter) {  // Reached cell center and beyond
						intraCellMovement.ValueRW.SetAtCenter();

						hoursDelta -= hoursToCellCenter;
					}
					else {  // Not reached cell center
						var distance = hoursDelta * speed;
						intraCellMovement.ValueRW.Advance(distance / cellEdgeDiameter_Km);

						hoursDelta = 0;
					}
				}
				else {  // Not reached target cell yet
					Assert.IsTrue(path[^1].Position == movementActivity.TargetPosition);

					var distanceToCellEdge = intraCellMovement.ValueRO.DistanceToFinalEdge * cellEdgeDiameter_Km;
					var hoursToCellEdge = distanceToCellEdge / speed;

					if (hoursDelta > hoursToCellEdge) {  // Reached cell edge and beyond
						intraCellMovement.ValueRW.SetAtStartEdge(previousPosition: tilePosition.ValueRW.Position);
						tilePosition.ValueRW.Position = path[0].Position;
						path.RemoveAt(0);

						hoursDelta -= hoursToCellEdge;
					}
					else {  // Not reached cell edge
						var distance = hoursDelta * speed;
						intraCellMovement.ValueRW.Advance(distance / cellEdgeDiameter_Km);

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
