using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Presentation {



[UpdateInGroup(typeof(LocalTransformPresentation))]
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

		foreach (var (position, intraCellMovement, path,
			         localTransform)
		         in SystemAPI.Query<
			         MapPosition, RefRO<IntraCellMovement>, DynamicBuffer<PathTile>,
			         RefRW<LocalTransform>
			         >()
			         .WithAll<MovementActivity>())
		{
			Vector3 point;

			if (intraCellMovement.ValueRO.IsBeforeCenter) {
				point = hexLayout.GetLerpPoint(intraCellMovement.ValueRO.PreviousPosition, position,
				                               intraCellMovement.ValueRO.PositionLerpParameter);
			}
			else if (intraCellMovement.ValueRO.IsAfterCenter) {
				point = hexLayout.GetLerpPoint(position, path[0].Position,
				                               intraCellMovement.ValueRO.PositionLerpParameter);
			}
			else {
				point = hexLayout.GetPoint(position);
			}

			localTransform.ValueRW = LocalTransform.FromPosition(point);
		}

		foreach (var (position,
			         localTransform)
		         in SystemAPI.Query<
			         MapPosition,
			         RefRW<LocalTransform>
			         >()
			         .WithDisabled<MovementActivity>())
		{
			localTransform.ValueRW = LocalTransform.FromPosition(
				hexLayout.GetPoint(position));
		}
	}
}



}
