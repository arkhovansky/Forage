using System.Collections.Generic;
using System.Linq;

using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

using Lib.Util;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Presentation;
using App.Game.Database;
using App.Game.ECS.Resource.Plant.Presentation.Components;
using App.Game.ECS.Resource.Plant.Presentation.Components.Config;
using App.Game.ECS.Resource.Plant.Presentation.Systems;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



public class ResourcePresentationInitializer : IResourcePresentationInitializer
{
	private readonly IResourceType_GraphicalPresentation_Repository _resourceTypePresentationRepository;

	private readonly PlantResourcePresentation_Config _plantResourcePresentation_Config;

	private readonly IEcsHelper _ecsHelper;

	//----------------------------------------------------------------------------------------------


	public ResourcePresentationInitializer(
		IResourceType_GraphicalPresentation_Repository resourceTypePresentationRepository,
		PlantResourcePresentation_Config plantResourcePresentation_Config,
		IEcsHelper ecsHelper)
	{
		_resourceTypePresentationRepository = resourceTypePresentationRepository;
		_plantResourcePresentation_Config = plantResourcePresentation_Config;
		_ecsHelper = ecsHelper;
	}


	//----------------------------------------------------------------------------------------------
	// IResourcePresentationInitializer


	public void Init(ISet<ResourceTypeId> resourceTypeIds)
	{
		var world = World.DefaultGameObjectInjectionWorld;
		var em = world.EntityManager;

		var singletonEntity = _ecsHelper.GetSingletonEntity();

		var maxResourceTypeId = resourceTypeIds.Max();

		var mmiArray = em.AddBuffer<ResourceIcon_MaterialMeshInfo>(singletonEntity);
		mmiArray.Resize((int) maxResourceTypeId + 1, NativeArrayOptions.ClearMemory);

		var meshes = new SetList<Mesh>();
		var materials = new SetList<Material>();

		foreach (var resourceTypeId in resourceTypeIds) {
			var resourceType = _resourceTypePresentationRepository.Get(resourceTypeId);

			var meshIndex = meshes.Add(resourceType.Mesh);
			var materialIndex = materials.Add(resourceType.Material);

			mmiArray[(int) resourceTypeId] = new ResourceIcon_MaterialMeshInfo(
				MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, meshIndex));
		}

		var prototype = em.CreateEntity(typeof(LocalTransform), typeof(Prefab));
		RenderMeshUtility.AddComponents(
			prototype,
			em,
			Create_RenderMeshDescription(),
			new RenderMeshArray(materials.ToArray(), meshes.ToArray()),
			MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
		em.SetName(prototype, "Resource icon prototype");

		var system = world.GetExistingSystem<PlantResourcePresentation>();
		em.AddComponentData(system, new ResourceIcon_Prototype(prototype));
		em.AddComponentData(system, _plantResourcePresentation_Config);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private RenderMeshDescription Create_RenderMeshDescription()
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
