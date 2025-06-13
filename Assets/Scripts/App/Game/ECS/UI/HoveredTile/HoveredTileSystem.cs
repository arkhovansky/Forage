using System;

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

using Lib.Grid;

using App.Game.ECS.Map.Components;
using App.Game.ECS.UI.HoveredTile.Components;



namespace App.Game.ECS.UI.HoveredTile {



[UpdateInGroup(typeof(StructuralChangePresentationSystemGroup))]
public partial struct HoveredTileSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<HoveredTileChanged_Event>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<HoveredTileChanged_Event>();

		RemoveOldHoveredTilePresentation(state.EntityManager, singletonEntity);

		var hoveredTileChangedEvent = state.EntityManager.GetComponentData<HoveredTileChanged_Event>(singletonEntity);
		AxialPosition? hoveredPosition = hoveredTileChangedEvent.NewPosition;

		if (hoveredPosition.HasValue) {  // There is the new hovered tile
			Entity? hoveredTileEntity = null;

			foreach (var (pos, entity) in SystemAPI.Query<MapPosition>().WithEntityAccess()) {
				if (pos == hoveredPosition.Value) {
					hoveredTileEntity = entity;
					break;
				}
			}

			if (hoveredTileEntity == null)
				throw new Exception($"Tile with position {hoveredPosition.Value} not found");

			AddNewHoveredTilePresentation(state.EntityManager, hoveredTileEntity.Value);

			state.EntityManager.AddComponentData(singletonEntity, new HoveredTileEntity(hoveredTileEntity.Value));

		}
		else {  // No tile is hovered
			state.EntityManager.RemoveComponent<HoveredTileEntity>(singletonEntity);
		}

		state.EntityManager.RemoveComponent<HoveredTileChanged_Event>(singletonEntity);
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }


	private void RemoveOldHoveredTilePresentation(EntityManager entityManager, Entity singletonEntity)
	{
		if (entityManager.HasComponent<HoveredTileEntity>(singletonEntity)) {
			var oldTileEntity = entityManager.GetComponentData<HoveredTileEntity>(singletonEntity).Entity;
			entityManager.RemoveComponent<URPMaterialPropertyBaseColor>(oldTileEntity);
		}
	}


	private void AddNewHoveredTilePresentation(EntityManager entityManager, Entity tileEntity)
	{
		entityManager.AddComponentData(tileEntity,
			new URPMaterialPropertyBaseColor {Value = new float4(0.8f, 0.8f, 0.8f, 1)});
	}
}



}
