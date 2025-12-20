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
using Lib.Grid.Spatial;
using Lib.Grid.Visual;
using Lib.Util;

using App.Game.Database;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Terrain.Components;
using App.Infrastructure.Common.Contracts.Database.Presentation;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



public class TerrainInitializer : ITerrainInitializer
{
	private readonly HexGridLayout_3D _gridLayout;

	private readonly ITerrainTypePresentationRepository _terrainTypePresentationRepository;
	private readonly IMapPresentationRepository _mapPresentationRepository;

	private readonly IEcsHelper _ecsHelper;



	public TerrainInitializer(
		HexGridLayout_3D gridLayout,
		ITerrainTypePresentationRepository terrainTypePresentationRepository,
		IMapPresentationRepository mapPresentationRepository,
		IEcsHelper ecsHelper)
	{
		_gridLayout = gridLayout;
		_terrainTypePresentationRepository = terrainTypePresentationRepository;
		_mapPresentationRepository = mapPresentationRepository;
		_ecsHelper = ecsHelper;
	}



	public void Init(IReadOnlyList<TerrainTypeId> tileTerrainTypes,
	                 RectangularHexMap map,
	                 float tilePhysicalInnerDiameter)
	{
		CreateTiles(tileTerrainTypes, map);
		CreatePhysicalMapParameters(tilePhysicalInnerDiameter);
		CreateGridLines(map);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void CreateTiles(IReadOnlyList<TerrainTypeId> tileTerrainTypes,
	                         RectangularHexMap map)
	{
		var (renderMeshArray, materialMeshInfo_By_TerrainType) = PrepareMeshMaterialData(tileTerrainTypes);

		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prototype = entityManager.CreateEntity(typeof(TerrainTile), typeof(MapPosition));
		entityManager.AddComponent<TilePlantResource>(prototype);
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
					new LocalToWorld {Value = float4x4.Translate(_gridLayout.GetPoint(axialPosition))});
				entityManager.SetComponentData(entity, materialMeshInfo_By_TerrainType[terrainTypeId]);

				entityManager.SetName(entity, $"Terrain {axialPosition}");
			}

			CreateMapBuffer(clonedEntities, map);
		}

		entityManager.DestroyEntity(prototype);
	}


	private
		(RenderMeshArray, Dictionary<TerrainTypeId, MaterialMeshInfo>)
		PrepareMeshMaterialData(IReadOnlyList<TerrainTypeId> tileTerrainTypes)
	{
		var terrainTypeIds = tileTerrainTypes.ToHashSet();

		var materialMeshInfo_By_TerrainType = new Dictionary<TerrainTypeId, MaterialMeshInfo>();
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


	private void CreateMapBuffer(NativeArray<Entity> tileEntities, RectangularHexMap map)
	{
		Assert.IsTrue(tileEntities.Length == map.CellCount);

		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		var mapBuffer = em.AddBuffer<MapTileEntity>(_ecsHelper.GetSingletonEntity());
		mapBuffer.EnsureCapacity((int)map.CellCount);
		foreach (var entity in tileEntities)
			mapBuffer.Add(new MapTileEntity(entity));
	}


	private void CreatePhysicalMapParameters(float tilePhysicalInnerDiameter)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.AddComponentData(_ecsHelper.GetSingletonEntity(),
		                    new PhysicalMapParameters(tilePhysicalInnerDiameter));
	}


	private void CreateGridLines(RectangularHexMap map)
	{
		var spatialMap = new Spatial_RectangularHexMap_3D(map, _gridLayout);

		var mesh = spatialMap.GetGridLinesMesh();
		var material = _mapPresentationRepository.Get_GridLinesMaterial();
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

		entityManager.SetName(entity, "Grid lines");
	}
}



}
