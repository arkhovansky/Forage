using System.Collections.Generic;
using System.Linq;

using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

using Lib.Util;

using App.Game.ECS.Resource.Plant.Presentation.Components;
using App.Services;
using App.Services.Resources;

using Unity.Collections;



namespace App.Infrastructure.ECS.Services.RunningGameInitializer_Impl {



public class ResourcePresentationInitializer : IResourcePresentationInitializer
{
	private readonly IResourceTypePresentationRepository _resourceTypePresentationRepository;


	public ResourcePresentationInitializer(IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		_resourceTypePresentationRepository = resourceTypePresentationRepository;
	}


	public void Init(ISet<uint> resourceTypeIds)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = EcsService.GetSingletonEntity();

		var maxResourceTypeId = resourceTypeIds.Max();

		var mmiArray = em.AddBuffer<ResourceIcon_MaterialMeshInfo>(singletonEntity);
		mmiArray.Resize((int)maxResourceTypeId + 1, NativeArrayOptions.ClearMemory);

		var meshes = new SetList<Mesh>();
		var materials = new SetList<Material>();

		for (uint resourceTypeId = 0; resourceTypeId <= maxResourceTypeId; ++resourceTypeId) {
			if (resourceTypeIds.Contains(resourceTypeId)) {
				var resourceType = _resourceTypePresentationRepository.Get(resourceTypeId);

				var meshIndex = meshes.Add(resourceType.Mesh);
				var materialIndex = materials.Add(resourceType.Material);

				mmiArray[(int)resourceTypeId] = new ResourceIcon_MaterialMeshInfo(
					MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, meshIndex));
			}
		}

		em.AddComponentData(singletonEntity,
		                    new ResourceIcons_RenderMeshArray
			                    {Value = new RenderMeshArray(materials.ToArray(), meshes.ToArray())});
	}
}



}
