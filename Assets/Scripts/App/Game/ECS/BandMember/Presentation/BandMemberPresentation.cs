using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Components;
using App.Game.ECS.Components.Singletons;



namespace App.Game.ECS.BandMember.Presentation {



[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct BandMemberPresentation : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Camp.Components.Camp>();
	}



	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var hexLayout = SystemAPI.GetSingleton<HexLayout3D_Component>().Layout;

		foreach (var (tilePosition, intraCellMovement, path,
			         localTransform)
		         in SystemAPI.Query<
			         TilePosition, RefRO<IntraCellMovement>, DynamicBuffer<PathTile>,
			         RefRW<LocalTransform>
			         >()
			         .WithAll<MovementActivity>())
		{
			Vector3 point;

			if (intraCellMovement.ValueRO.IsBeforeCenter) {
				point = hexLayout.GetLerpPoint(intraCellMovement.ValueRO.PreviousPosition, tilePosition.Position,
				                               intraCellMovement.ValueRO.PositionLerpParameter);
			}
			else if (intraCellMovement.ValueRO.IsAfterCenter) {
				point = hexLayout.GetLerpPoint(tilePosition.Position, path[0].Position,
				                               intraCellMovement.ValueRO.PositionLerpParameter);
			}
			else {
				point = hexLayout.GetPoint(tilePosition.Position);
			}

			localTransform.ValueRW = LocalTransform.FromPosition(point);
		}

		foreach (var (tilePosition,
			         localTransform)
		         in SystemAPI.Query<
			         TilePosition,
			         RefRW<LocalTransform>
			         >()
			         .WithDisabled<MovementActivity>())
		{
			localTransform.ValueRW = LocalTransform.FromPosition(
				hexLayout.GetPoint(tilePosition.Position));
		}
	}
}



}
