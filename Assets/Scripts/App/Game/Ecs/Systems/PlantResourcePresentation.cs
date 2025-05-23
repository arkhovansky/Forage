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
using Lib.VisualGrid;
using Lib.Util;

using App.Game.Ecs.Components;
using App.Game.Ecs.Components.Singletons.YearPeriod;
using App.Services.Resources;



namespace App.Game.Ecs.Systems {



[UpdateAfter(typeof(PlantCycle))]
public partial class PlantResourcePresentation : SystemBase
{
	private struct ResourceIconsCreationData
	{
		public uint ResourceType;
		public AxialPosition TilePosition;
		public float RipeBiomass;
	}

	private struct ResourceIconsRemovalData
	{
		public uint ResourceType;
		public AxialPosition TilePosition;
	}


	private HexLayout3D _grid;

	private RenderMeshArray _renderMeshArray;

	private Dictionary<uint, MaterialMeshInfo>? _resourceType_To_MaterialMeshInfo;

	private bool _initialized;



	public void InitForScene(
		HexLayout3D grid,
		IResourceTypePresentationRepository resourceTypePresentationRepository,
		IReadOnlyList<uint> resourceTypes)
	{
		_grid = grid;

		(_renderMeshArray, _resourceType_To_MaterialMeshInfo) =
			PrepareMeshMaterialData(resourceTypes, resourceTypePresentationRepository);

		_initialized = true;
	}



	protected override void OnUpdate()
	{
		if (!_initialized)  // Should not happen
			return;

		var removalData = new List<ResourceIconsRemovalData>();
		var creationData = new List<ResourceIconsCreationData>();

		foreach (var (resource, tilePosition, ripeBiomass)
		         in SystemAPI.Query<RefRO<PlantResource>, RefRO<TilePosition>, RefRO<RemainingRipeBiomass>>())
		{
			if (SystemAPI.HasSingleton<YearPeriodChanged_Event>()) {
				if (ripeBiomass.ValueRO.Value > 0) {
					creationData.Add(new ResourceIconsCreationData {
						ResourceType = resource.ValueRO.TypeId,
						TilePosition = tilePosition.ValueRO.Position,
						RipeBiomass = ripeBiomass.ValueRO.Value
					});
				}
				else {
					removalData.Add(new ResourceIconsRemovalData {
						ResourceType = resource.ValueRO.TypeId,
						TilePosition = tilePosition.ValueRO.Position
					});
				}
			}
		}

		RemoveIcons(removalData);
		CreateIcons(creationData);
	}



	private
		(RenderMeshArray, Dictionary<uint, MaterialMeshInfo>)
		PrepareMeshMaterialData(IReadOnlyList<uint> resourceTypes,
		                        IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		var resourceTypeIds = resourceTypes.ToHashSet();

		var resourceType_To_MaterialMeshInfo = new Dictionary<uint, MaterialMeshInfo>();
		var meshes = new SetList<Mesh>();
		var materials = new SetList<Material>();

		foreach (var resourceTypeId in resourceTypeIds) {
			var resourceType = resourceTypePresentationRepository.Get(resourceTypeId);

			var meshIndex = meshes.Add(resourceType.Mesh);
			var materialIndex = materials.Add(resourceType.Material);

			resourceType_To_MaterialMeshInfo[resourceTypeId] =
				MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, meshIndex);
		}

		var renderMeshArray = new RenderMeshArray(materials.ToArray(), meshes.ToArray());

		return (renderMeshArray, resourceType_To_MaterialMeshInfo);
	}



	private void CreateIcons(IReadOnlyList<ResourceIconsCreationData> creationData)
	{
		var filterSettings = RenderFilterSettings.Default;
		filterSettings.ShadowCastingMode = ShadowCastingMode.Off;
		filterSettings.ReceiveShadows = false;

		var renderMeshDescription = new RenderMeshDescription {
			FilterSettings = filterSettings,
			LightProbeUsage = LightProbeUsage.Off
		};


		var prototype = EntityManager.CreateEntity();
		EntityManager.AddComponent<PlantResourceType>(prototype);
		EntityManager.AddComponent<TilePosition>(prototype);
		EntityManager.AddComponent<LocalTransform>(prototype);
		RenderMeshUtility.AddComponents(
			prototype,
			EntityManager,
			renderMeshDescription,
			_renderMeshArray,
			MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

		var (perResourceCounts, totalCount) = CountIcons(creationData);

		var clonedEntities = new NativeArray<Entity>((int)totalCount, Allocator.Temp);
		EntityManager.Instantiate(prototype, clonedEntities);

		int iIcon = 0;
		for (var iResource = 0; iResource < creationData.Count; iResource++) {
			var resourceData = creationData[iResource];
			var iconCount = perResourceCounts[iResource];

			var materialMeshInfo = _resourceType_To_MaterialMeshInfo![resourceData.ResourceType];

			for (var iResourceIcon = 0; iResourceIcon < iconCount; iResourceIcon++) {
				var entity = clonedEntities[iIcon];

				EntityManager.SetComponentData(entity, new PlantResourceType {TypeId = resourceData.ResourceType});
				EntityManager.SetComponentData(entity, new TilePosition(resourceData.TilePosition));

				var inTilePosition = new Vector2((iResourceIcon + 1) * 0.1f, 0.5f);
				var localTransform = _grid.GetCellLocalTransform(resourceData.TilePosition)
					.Translate(new float3(inTilePosition.x, 0.01f, -inTilePosition.y))
					.ApplyScale(0.5f);
				EntityManager.SetComponentData(entity, localTransform);

				EntityManager.SetComponentData(entity, materialMeshInfo);

				++iIcon;
			}
		}

		clonedEntities.Dispose();

		EntityManager.DestroyEntity(prototype);
	}



	private void RemoveIcons(IReadOnlyList<ResourceIconsRemovalData> removalData)
	{
		var entities = new List<Entity>();

		foreach (var (resourceTypeId_, tilePosition_, entity)
		         in SystemAPI.Query<RefRO<PlantResourceType>, RefRO<TilePosition>>().WithEntityAccess())
		{
			entities.AddRange(
				from resourceRemovalData in removalData
				where resourceTypeId_.ValueRO.TypeId == resourceRemovalData.ResourceType &&
				      tilePosition_.ValueRO.Position == resourceRemovalData.TilePosition
				select entity);
		}

		foreach (var entity in entities)
			EntityManager.DestroyEntity(entity);
	}



	private (IReadOnlyList<uint>, uint) CountIcons(IReadOnlyList<ResourceIconsCreationData> creationData)
	{
		var perResourceCounts = new uint[creationData.Count];
		uint totalCount = 0;

		for (var i = 0; i < creationData.Count; i++) {
			perResourceCounts[i] = (uint) (creationData[i].RipeBiomass / 100);
			totalCount += perResourceCounts[i];
		}

		return (perResourceCounts, totalCount);
	}
}



}
