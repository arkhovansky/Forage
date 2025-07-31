using System.Collections.Generic;
using System.Linq;

using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

using UnityEngine;
using UnityEngine.Rendering;

using Lib.Grid;
using Lib.Util;
using Lib.VisualGrid;

using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Terrain.Components;



namespace App.Services.Terrain {



public class TerrainInitializer : ITerrainInitializer
{
	private readonly HexLayout3D _grid;

	private readonly ITerrainTypePresentationRepository _terrainTypePresentationRepository;



	public TerrainInitializer(
		HexLayout3D grid,
		ITerrainTypePresentationRepository terrainTypePresentationRepository)
	{
		_grid = grid;
		_terrainTypePresentationRepository = terrainTypePresentationRepository;
	}



	public void Init(IReadOnlyList<uint> tileTerrainTypes,
	                 RectangularHexMap map)
	{
		CreateTiles(tileTerrainTypes, map);
		CreateGridLines(map);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void CreateTiles(IReadOnlyList<uint> tileTerrainTypes,
	                         RectangularHexMap map)
	{
		var (renderMeshArray, materialMeshInfo_By_TerrainType) = PrepareMeshMaterialData(tileTerrainTypes);

		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prototype = entityManager.CreateEntity(typeof(TerrainTile), typeof(MapPosition));
		RenderMeshUtility.AddComponents(
			prototype,
			entityManager,
			GetRenderMeshDescription(),
			renderMeshArray,
			MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

		var terrainTileCount = tileTerrainTypes.Count;

		using (var clonedEntities = new NativeArray<Entity>(terrainTileCount, Allocator.Temp)) {
			entityManager.Instantiate(prototype, clonedEntities);

			for (var i = 0; i < terrainTileCount; ++i) {
				var entity = clonedEntities[i];
				var terrainTypeId = tileTerrainTypes[i];
				var axialPosition = map.AxialPositionFromCellIndex((uint)i);

				entityManager.SetComponentData(entity, new TerrainTile(terrainTypeId));
				entityManager.SetComponentData(entity, new MapPosition(axialPosition));
				entityManager.SetComponentData(entity,
					new LocalToWorld {Value = float4x4.Translate(_grid.GetPoint(axialPosition))});
				entityManager.SetComponentData(entity, materialMeshInfo_By_TerrainType[terrainTypeId]);
			}

			CreateMapBuffer(clonedEntities, map);
		}

		entityManager.DestroyEntity(prototype);
	}


	private
		(RenderMeshArray, Dictionary<uint, MaterialMeshInfo>)
		PrepareMeshMaterialData(IReadOnlyList<uint> tileTerrainTypes)
	{
		var terrainTypeIds = tileTerrainTypes.ToHashSet();

		var materialMeshInfo_By_TerrainType = new Dictionary<uint, MaterialMeshInfo>();
		var meshes = new SetList<Mesh>();
		var materials = new SetList<Material>();

		foreach (var terrainTypeId in terrainTypeIds) {
			var terrainType = _terrainTypePresentationRepository.Get(terrainTypeId);

			var meshIndex = meshes.Add(terrainType.Mesh);
			var materialIndex = materials.Add(terrainType.Material);

			materialMeshInfo_By_TerrainType[terrainTypeId] =
				MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, meshIndex);
		}

		var renderMeshArray = new RenderMeshArray(materials.ToArray(), meshes.ToArray());

		return (renderMeshArray, materialMeshInfo_By_TerrainType);
	}


	private RenderMeshDescription GetRenderMeshDescription()
	{
		var filterSettings = RenderFilterSettings.Default;
		filterSettings.ShadowCastingMode = ShadowCastingMode.Off;
		filterSettings.ReceiveShadows = false;

		return new RenderMeshDescription {
			FilterSettings = filterSettings,
			LightProbeUsage = LightProbeUsage.Off
		};
	}


	private static void CreateMapBuffer(NativeArray<Entity> tileEntities, RectangularHexMap map)
	{
		Assert.IsTrue(tileEntities.Length == map.CellCount);

		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		var mapBuffer = em.AddBuffer<MapTileEntity>(EcsService.GetSingletonEntity());
		mapBuffer.EnsureCapacity((int)map.CellCount);
		foreach (var entity in tileEntities)
			mapBuffer.Add(new MapTileEntity(entity));
	}


	private void CreateGridLines(RectangularHexMap map)
	{
		var visualMap = new VisualRectangularHexMap3D(map, _grid);

		var mesh = visualMap.GetGridLinesMesh();
		var material = UnityEngine.Resources.Load<Material>("Materials/GridLines");
		var renderMeshArray = new RenderMeshArray(new [] { material }, new [] { mesh });

		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entity = entityManager.CreateEntity();

		RenderMeshUtility.AddComponents(
			entity,
			entityManager,
			GetRenderMeshDescription(),
			renderMeshArray,
			MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

		entityManager.SetComponentData(entity, new LocalToWorld {Value = float4x4.Translate(new float3(0, 0.005f, 0))});
	}
}



}
