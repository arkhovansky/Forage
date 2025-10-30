using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

using Lib.Grid;

using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.UI.HoveredTile.Components;



namespace App.Game.ECS.UI.HoveredTile {



[UpdateInGroup(typeof(StructuralChangePresentation))]
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
			var hoveredTileEntity = GetTileEntity(hoveredPosition.Value);

			AddNewHoveredTilePresentation(state.EntityManager, hoveredTileEntity);

			state.EntityManager.AddComponentData(singletonEntity, new HoveredTileEntity(hoveredTileEntity));
		}
		else {  // No tile is hovered
			state.EntityManager.RemoveComponent<HoveredTileEntity>(singletonEntity);
		}

		state.EntityManager.RemoveComponent<HoveredTileChanged_Event>(singletonEntity);
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }


	private Entity GetTileEntity(AxialPosition position)
	{
		var map = SystemAPI.GetSingleton<Map.Components.Singletons.Map>().Value;
		Assert.IsTrue(map.Contains(position));
		var mapBuffer = SystemAPI.GetSingletonBuffer<MapTileEntity>();
		return mapBuffer[(int)map.CellIndexFrom(position)];
	}


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
