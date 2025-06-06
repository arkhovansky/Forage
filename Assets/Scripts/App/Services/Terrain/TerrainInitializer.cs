using System.Collections.Generic;
using System.Linq;

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
		var (renderMeshArray, materialMeshInfo_By_TerrainType) = PrepareMeshMaterialData(tileTerrainTypes);

		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prototype = entityManager.CreateEntity(typeof(TerrainTile), typeof(TilePosition));
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
				var axialPosition = tilePositions[i];

				entityManager.SetComponentData(entity, new TerrainTile(terrainTypeId));
				entityManager.SetComponentData(entity, new TilePosition(axialPosition));
				entityManager.SetComponentData(entity,
					new LocalToWorld {Value = float4x4.Translate(_grid.GetPoint(axialPosition))});
				entityManager.SetComponentData(entity, materialMeshInfo_By_TerrainType[terrainTypeId]);
			}
		}

		entityManager.DestroyEntity(prototype);
	}


	//----------------------------------------------------------------------------------------------
	// private


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
}



}
