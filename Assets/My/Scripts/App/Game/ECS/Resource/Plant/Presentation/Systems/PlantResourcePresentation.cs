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

using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Resource.Plant.Presentation.Components;
using App.Game.ECS.SystemGroups;

using Random = UnityEngine.Random;



namespace App.Game.ECS.Resource.Plant.Presentation.Systems {



[UpdateInGroup(typeof(LocalTransformPresentation))]
public partial class PlantResourcePresentation : SystemBase
{
	private struct ResourceIconsCreationData
	{
		public uint ResourceType;
		public AxialPosition MapPosition;
		public int IconCountToCreate;
		public Entity ResourceEntity;
	}



	/// <summary>
	/// Icon side length relative to inner cell diameter.
	/// </summary>
	private const float RelativeIconSize = 0.2f;


	private RenderMeshDescription _renderMeshDescription;



	protected override void OnCreate()
	{
		base.OnCreate();

		_renderMeshDescription = CreateRenderMeshDescription();
	}



	protected override void OnUpdate()
	{
		var creationData = new List<ResourceIconsCreationData>();
		var entitiesToDestroy = new NativeList<Entity>(Allocator.Temp);

		foreach (var (resource, position, ripeBiomass, icons, entity)
		         in SystemAPI.Query<
			         RefRO<PlantResource>, MapPosition, RipeBiomass, DynamicBuffer<ResourceIcon>>()
			         .WithEntityAccess())
		{
			const uint biomassPerIcon = 20;
			int neededIconCount = Mathf.CeilToInt(ripeBiomass.Value / biomassPerIcon);

			if (icons.Length == neededIconCount)
				continue;

			if (icons.Length < neededIconCount) {
				creationData.Add(new ResourceIconsCreationData {
					ResourceType = resource.ValueRO.TypeId,
					MapPosition = position,
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
		DestroyIcons(entitiesToDestroy);
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



	private void CreateIcons(IReadOnlyList<ResourceIconsCreationData> creationData)
	{
		var renderMeshArray = SystemAPI.ManagedAPI.GetSingleton<ResourceIcons_RenderMeshArray>().Value;
		var hexLayout = SystemAPI.GetSingleton<HexLayout3D_Component>().Layout;

		var prototype = EntityManager.CreateEntity(typeof(LocalTransform));
		RenderMeshUtility.AddComponents(prototype,
		                                EntityManager, _renderMeshDescription, renderMeshArray,
		                                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

		// Placed after prototype creation, or the buffer is invalidated by structural change (why?)
		var mmiBuffer = SystemAPI.GetSingletonBuffer<ResourceIcon_MaterialMeshInfo>(true);

		uint totalCount = CountIcons(creationData);

		var clonedEntities = new NativeArray<Entity>((int)totalCount, Allocator.Temp);
		EntityManager.Instantiate(prototype, clonedEntities);

		for (int iResource = 0, iIcon = 0; iResource < creationData.Count; iResource++) {
			var resourceData = creationData[iResource];

			var materialMeshInfo = mmiBuffer[(int)resourceData.ResourceType].Value;

			var resourceIconsBuffer = EntityManager.GetBuffer<ResourceIcon>(resourceData.ResourceEntity);

			for (var iResourceIcon = 0; iResourceIcon < resourceData.IconCountToCreate; iResourceIcon++) {
				var entity = clonedEntities[iIcon++];
				uint iconIndexInResource = (uint) resourceIconsBuffer.Length;

				EntityManager.SetComponentData(entity,
					GetIconLocalTransform(resourceData.MapPosition, iconIndexInResource, hexLayout));

				EntityManager.SetName(entity,
					"Resource icon: " +
					$"type {resourceData.ResourceType} {resourceData.MapPosition} - {iconIndexInResource}");

				EntityManager.SetComponentData(entity, materialMeshInfo);

				resourceIconsBuffer.Add(new ResourceIcon(entity));
			}
		}

		clonedEntities.Dispose();

		EntityManager.DestroyEntity(prototype);
	}


	private void DestroyIcons(NativeList<Entity> iconEntities)
	{
		EntityManager.DestroyEntity(iconEntities.AsArray());
	}


	private uint CountIcons(IReadOnlyList<ResourceIconsCreationData> creationData)
	{
		return (uint) creationData.Sum(d => d.IconCountToCreate);
	}


	private LocalTransform GetIconLocalTransform(AxialPosition tilePosition, uint iconIndexInResource,
	                                             HexLayout3D hexLayout)
	{
		var inTilePosition = GetIconInTilePosition(iconIndexInResource, hexLayout);
		return hexLayout.GetCellLocalTransform(tilePosition)
			.Translate(new float3(inTilePosition.x, 0.01f, inTilePosition.y))
			.ApplyScale(hexLayout.InnerCellRadius * 2 * RelativeIconSize);  // Assume icon mesh size is 1x1
	}

	// ReSharper disable once UnusedParameter.Local
	private Vector2 GetIconInTilePosition(uint iconIndexInResource,
	                                      HexLayout3D hexLayout)
	{
		var iconRadius = hexLayout.InnerCellRadius * RelativeIconSize;
		var areaRadius = hexLayout.InnerCellRadius - iconRadius;
		return Random.insideUnitCircle * areaRadius;
	}
}



}
