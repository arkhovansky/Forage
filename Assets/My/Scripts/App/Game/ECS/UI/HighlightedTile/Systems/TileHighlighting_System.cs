using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

using Lib.Grid;

using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.UI.HighlightedTile.Components;



namespace App.Game.ECS.UI.HighlightedTile.Systems {



[UpdateInGroup(typeof(StructuralChangePresentation))]
public partial struct TileHighlighting_System : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<HighlightedTile_Changed_Event>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<HighlightedTile_Changed_Event>();

		RemoveOldHighlighting(state.EntityManager, singletonEntity);

		var changeEvent = state.EntityManager.GetComponentData<HighlightedTile_Changed_Event>(singletonEntity);
		AxialPosition? newPosition = changeEvent.NewPosition;

		if (newPosition.HasValue) {  // There is the new highlighted tile
			var tileEntity = GetTileEntity(newPosition.Value);

			SetNewHighlighting(state.EntityManager, tileEntity);

			state.EntityManager.AddComponentData(singletonEntity, new HighlightedTileEntity(tileEntity));
		}
		else {  // No highlighted tile
			state.EntityManager.RemoveComponent<HighlightedTileEntity>(singletonEntity);
		}

		state.EntityManager.RemoveComponent<HighlightedTile_Changed_Event>(singletonEntity);
	}



	private Entity GetTileEntity(AxialPosition position)
	{
		var map = SystemAPI.GetSingleton<Map.Components.Singletons.Map>().Value;
		Assert.IsTrue(map.Contains(position));
		var mapBuffer = SystemAPI.GetSingletonBuffer<MapTileEntity>();
		return mapBuffer[(int)map.CellIndexFrom(position)];
	}


	private void RemoveOldHighlighting(EntityManager entityManager, Entity singletonEntity)
	{
		if (entityManager.HasComponent<HighlightedTileEntity>(singletonEntity)) {
			var oldTileEntity = entityManager.GetComponentData<HighlightedTileEntity>(singletonEntity).Entity;
			entityManager.RemoveComponent<URPMaterialPropertyBaseColor>(oldTileEntity);
		}
	}


	private void SetNewHighlighting(EntityManager entityManager, Entity tileEntity)
	{
		entityManager.AddComponentData(tileEntity,
			new URPMaterialPropertyBaseColor {Value = new float4(0.8f, 0.8f, 0.8f, 1)});
	}
}



}
