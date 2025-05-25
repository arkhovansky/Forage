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


		// var ecb = new EntityCommandBuffer(Allocator.TempJob);

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
		// entityManager.AddComponent<MaterialColor>(prototype);
		entityManager.AddComponent<Static>(prototype);
		// entityManager.SetComponentData(prototype, new LocalToWorld { Value = float4x4.identity });

		// var bounds = new RenderBounds { Value = tileMesh.bounds.ToAABB() };

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

			// float3 hsvColor = map.Tile(i).TerrainTypeId switch {
			// 	3 => new float3(130, 80, 80),
			// 	4 => new float3(70, 80, 80),
			// 	5 => new float3(80, 80, 80),
			// 	_ => new float3()
			// };
			// var rgb = Color.HSVToRGB(hsvColor.x, hsvColor.y, hsvColor.z);
			// var color = new float4(rgb.r, rgb.g, rgb.b, 1);
			// entityManager.SetComponentData(entity, new MaterialColor { Value = color });

			// entityManager.SetComponentData(entity, bounds);
		}

		clonedEntities.Dispose();


		// Spawn most of the entities in a Burst job by cloning a pre-created prototype entity,
		// which can be either a Prefab or an entity created at run time like in this sample.
		// This is the fastest and most efficient way to create entities at run time.
		// var spawnJob = new SpawnJob {
		// 	Prototype = prototype,
		// 	Map = map,
		// 	Ecb = ecb.AsParallelWriter(),
		// 	MeshBounds = bounds,
		// 	//ObjectScale = ObjectScale,
		// };
		//
		// var spawnHandle = spawnJob.Schedule((int) map.TileCount, 128);
		// bounds.Dispose(spawnHandle);
		//
		// spawnHandle.Complete();
		//
		// ecb.Playback(entityManager);
		// ecb.Dispose();

		entityManager.DestroyEntity(prototype);
	}



	//----------------------------------------------------------------------------------------------
	// private


	// [GenerateTestsForBurstCompatibility]
	// private struct SpawnJob : IJobParallelFor
	// {
	// 	public Entity Prototype;
	// 	public ITerrainMap Map;
	// 	//public float ObjectScale;
	// 	public EntityCommandBuffer.ParallelWriter Ecb;
	//
	// 	[ReadOnly] public NativeArray<RenderBounds> MeshBounds;
	//
	//
	// 	public void Execute(int index)
	// 	{
	// 		var e = Ecb.Instantiate(index, Prototype);
	//
	// 		Ecb.SetComponent(index, e, Map.Tile((uint) index));
	//
	// 		Ecb.SetComponent(index, e, new LocalToWorld { Value = Map.Grid.GetCellLocalTransform((uint) index) });
	// 		//Ecb.SetComponent(index, e, new MaterialColor() { Value = ComputeColor(index) });
	// 		// MeshBounds must be set according to the actual mesh for culling to work.
	// 		int materialIndex = (int) Map.Tile((uint) index).TerrainTypeId;
	// 		Ecb.SetComponent(index, e, MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, 0));
	// 		Ecb.SetComponent(index, e, MeshBounds[0]);
	// 	}
	// }



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
