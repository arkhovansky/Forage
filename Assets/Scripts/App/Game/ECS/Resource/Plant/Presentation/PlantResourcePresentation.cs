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

using App.Game.ECS.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Resource.Plant.Presentation.Components;
using App.Services.Resources;



namespace App.Game.ECS.Resource.Plant.Presentation {



[UpdateBefore(typeof(TransformSystemGroup))]
public partial class PlantResourcePresentation : SystemBase
{
	private struct ResourceIconsCreationData
	{
		public uint ResourceType;
		public AxialPosition TilePosition;
		public int IconCountToCreate;
		public Entity ResourceEntity;
	}


	private HexLayout3D _grid;

	private RenderMeshDescription _renderMeshDescription;

	private RenderMeshArray _renderMeshArray;

	private Dictionary<uint, MaterialMeshInfo>? _materialMeshInfo_By_ResourceType;

	private bool _initialized;



	public void InitForScene(
		HexLayout3D grid,
		IResourceTypePresentationRepository resourceTypePresentationRepository,
		IReadOnlyList<uint> resourceTypes)
	{
		_grid = grid;

		InitMeshMaterialData(resourceTypes, resourceTypePresentationRepository);

		_initialized = true;
	}



	protected override void OnCreate()
	{
		base.OnCreate();

		_renderMeshDescription = CreateRenderMeshDescription();
	}



	protected override void OnUpdate()
	{
		if (!_initialized)  // Should not happen
			return;

		var creationData = new List<ResourceIconsCreationData>();
		var entitiesToDestroy = new NativeList<Entity>(Allocator.Temp);

		foreach (var (resource, tilePosition, ripeBiomass, icons, entity)
		         in SystemAPI.Query<
			         RefRO<PlantResource>, TilePosition, RipeBiomass, DynamicBuffer<ResourceIcon>>()
			         .WithEntityAccess())
		{
			const uint biomassPerIcon = 10;
			int neededIconCount = Mathf.CeilToInt(ripeBiomass.Value / biomassPerIcon);

			if (icons.Length == neededIconCount)
				continue;

			if (icons.Length < neededIconCount) {
				creationData.Add(new ResourceIconsCreationData {
					ResourceType = resource.ValueRO.TypeId,
					TilePosition = tilePosition.Position,
					IconCountToCreate = neededIconCount - icons.Length,
					ResourceEntity = entity
				});
			}
			else {
				for (var i = neededIconCount; i < icons.Length; i++)
					entitiesToDestroy.Add(icons[i].Entity);

				var icons_ = icons;
				icons_.Length = neededIconCount;
			}
		}

		CreateIcons(creationData);
		EntityManager.DestroyEntity(entitiesToDestroy.AsArray());
	}



	private RenderMeshDescription CreateRenderMeshDescription()
	{
		var filterSettings = RenderFilterSettings.Default;
		filterSettings.ShadowCastingMode = ShadowCastingMode.Off;
		filterSettings.ReceiveShadows = false;

		return new RenderMeshDescription {
			FilterSettings = filterSettings,
			LightProbeUsage = LightProbeUsage.Off
		};
	}



	private void InitMeshMaterialData(IReadOnlyList<uint> resourceTypes,
	                                  IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		var resourceTypeIds = resourceTypes.ToHashSet();

		_materialMeshInfo_By_ResourceType = new Dictionary<uint, MaterialMeshInfo>();
		var meshes = new SetList<Mesh>();
		var materials = new SetList<Material>();

		foreach (var resourceTypeId in resourceTypeIds) {
			var resourceType = resourceTypePresentationRepository.Get(resourceTypeId);

			var meshIndex = meshes.Add(resourceType.Mesh);
			var materialIndex = materials.Add(resourceType.Material);

			_materialMeshInfo_By_ResourceType[resourceTypeId] =
				MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, meshIndex);
		}

		_renderMeshArray = new RenderMeshArray(materials.ToArray(), meshes.ToArray());
	}



	private void CreateIcons(IReadOnlyList<ResourceIconsCreationData> creationData)
	{
		var prototype = EntityManager.CreateEntity(typeof(LocalTransform));
		RenderMeshUtility.AddComponents(prototype,
		                                EntityManager, _renderMeshDescription, _renderMeshArray,
		                                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

		uint totalCount = CountIcons(creationData);

		var clonedEntities = new NativeArray<Entity>((int)totalCount, Allocator.Temp);
		EntityManager.Instantiate(prototype, clonedEntities);

		for (int iResource = 0, iIcon = 0; iResource < creationData.Count; iResource++) {
			var resourceData = creationData[iResource];

			var materialMeshInfo = _materialMeshInfo_By_ResourceType![resourceData.ResourceType];

			var resourceIconsBuffer = EntityManager.GetBuffer<ResourceIcon>(resourceData.ResourceEntity);

			for (var iResourceIcon = 0; iResourceIcon < resourceData.IconCountToCreate; iResourceIcon++) {
				var entity = clonedEntities[iIcon++];
				uint iconIndexInResource = (uint) resourceIconsBuffer.Length;

				EntityManager.SetComponentData(entity,
					GetIconLocalTransform(resourceData.TilePosition, iconIndexInResource));

				EntityManager.SetComponentData(entity, materialMeshInfo);

				resourceIconsBuffer.Add(new ResourceIcon(entity));
			}
		}

		clonedEntities.Dispose();

		EntityManager.DestroyEntity(prototype);
	}



	private uint CountIcons(IReadOnlyList<ResourceIconsCreationData> creationData)
	{
		return (uint) creationData.Sum(d => d.IconCountToCreate);
	}


	private LocalTransform GetIconLocalTransform(AxialPosition tilePosition, uint iconIndexInResource)
	{
		var inTilePosition = GetIconInTilePosition(iconIndexInResource);
		return _grid.GetCellLocalTransform(tilePosition)
			.Translate(new float3(inTilePosition.x, 0.01f, inTilePosition.y))
			.ApplyScale(0.2f);
	}

	private Vector2 GetIconInTilePosition(uint iconIndexInResource)
	{
		return new Vector2(((iconIndexInResource + 1) * (1f / (10+1)) - 0.5f) * _grid.CellSize.x,
		                   0f);
	}
}



}
