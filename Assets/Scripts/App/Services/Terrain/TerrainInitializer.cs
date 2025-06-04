using System.Collections.Generic;
using System.Linq;

using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Rendering;
using Unity.Transforms;

using UnityEngine;
using UnityEngine.Rendering;

using Lib.Grid;
using Lib.Util;
using Lib.VisualGrid;

using App.Game.ECS.Components;
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



	public void Create(IReadOnlyList<uint> tileTerrainTypes,
	                   IReadOnlyList<AxialPosition> tilePositions)
	{
		var (renderMeshArray, terrainType_To_MaterialMeshInfo) = PrepareMeshMaterialData(tileTerrainTypes);


		var world = World.DefaultGameObjectInjectionWorld;
		var entityManager = world.EntityManager;

		var filterSettings = RenderFilterSettings.Default;
		filterSettings.ShadowCastingMode = ShadowCastingMode.Off;
		filterSettings.ReceiveShadows = false;

		var renderMeshDescription = new RenderMeshDescription {
			FilterSettings = filterSettings,
			LightProbeUsage = LightProbeUsage.Off
		};


		var prototype = entityManager.CreateEntity();
		entityManager.AddComponent<TerrainTile>(prototype);
		entityManager.AddComponent<TilePosition>(prototype);
		entityManager.AddComponent<LocalTransform>(prototype);
		RenderMeshUtility.AddComponents(
			prototype,
			entityManager,
			renderMeshDescription,
			renderMeshArray,
			MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
		entityManager.AddComponent<Static>(prototype);

		var terrainTileCount = tileTerrainTypes.Count;

		var clonedEntities = new NativeArray<Entity>(terrainTileCount, Allocator.Temp);
		entityManager.Instantiate(prototype, clonedEntities);

		for (uint i = 0; i < terrainTileCount; ++i) {
			var entity = clonedEntities[(int)i];

			var terrainTypeId = tileTerrainTypes[(int)i];

			var axialPosition = tilePositions[(int)i];

			entityManager.SetComponentData(entity, new TerrainTile(terrainTypeId));
			entityManager.SetComponentData(entity, new TilePosition(axialPosition));

			entityManager.SetComponentData(entity, _grid.GetCellLocalTransform(axialPosition));

			entityManager.SetComponentData(entity, terrainType_To_MaterialMeshInfo[terrainTypeId]);
		}

		clonedEntities.Dispose();


		entityManager.DestroyEntity(prototype);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private
		(RenderMeshArray, Dictionary<uint, MaterialMeshInfo>)
		PrepareMeshMaterialData(IReadOnlyList<uint> tileTerrainTypes)
	{
		var terrainTypeIds = tileTerrainTypes.ToHashSet();

		var terrainType_To_MaterialMeshInfo = new Dictionary<uint, MaterialMeshInfo>();
		var meshes = new SetList<Mesh>();
		var materials = new SetList<Material>();

		foreach (var terrainTypeId in terrainTypeIds) {
			var terrainType = _terrainTypePresentationRepository.Get(terrainTypeId);

			var meshIndex = meshes.Add(terrainType.Mesh);
			var materialIndex = materials.Add(terrainType.Material);

			terrainType_To_MaterialMeshInfo[terrainTypeId] =
				MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, meshIndex);
		}

		var renderMeshArray = new RenderMeshArray(materials.ToArray(), meshes.ToArray());

		return (renderMeshArray, terrainType_To_MaterialMeshInfo);
	}
}



}
