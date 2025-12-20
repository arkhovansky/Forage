using System.Collections.Generic;
using System.Linq;

using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

using Lib.Util;

using App.Game.Database;
using App.Game.ECS.Resource.Plant.Presentation.Components;
using App.Infrastructure.Common.Contracts.Database.Presentation;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



public class ResourcePresentationInitializer : IResourcePresentationInitializer
{
	private readonly IResourceTypePresentationRepository _resourceTypePresentationRepository;

	private readonly IEcsHelper _ecsHelper;



	public ResourcePresentationInitializer(IResourceTypePresentationRepository resourceTypePresentationRepository,
	                                       IEcsHelper ecsHelper)
	{
		_resourceTypePresentationRepository = resourceTypePresentationRepository;
		_ecsHelper = ecsHelper;
	}


	public void Init(ISet<ResourceTypeId> resourceTypeIds)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = _ecsHelper.GetSingletonEntity();

		var maxResourceTypeId = resourceTypeIds.Max();

		var mmiArray = em.AddBuffer<ResourceIcon_MaterialMeshInfo>(singletonEntity);
		mmiArray.Resize((int)maxResourceTypeId + 1, NativeArrayOptions.ClearMemory);

		var meshes = new SetList<Mesh>();
		var materials = new SetList<Material>();

		for (var resourceTypeId = 0; resourceTypeId <= (int) maxResourceTypeId; ++resourceTypeId) {
			if (resourceTypeIds.Contains((ResourceTypeId) resourceTypeId)) {
				var resourceType = _resourceTypePresentationRepository.Get((ResourceTypeId) resourceTypeId);

				var meshIndex = meshes.Add(resourceType.Mesh);
				var materialIndex = materials.Add(resourceType.Material);

				mmiArray[resourceTypeId] = new ResourceIcon_MaterialMeshInfo(
					MaterialMeshInfo.FromRenderMeshArrayIndices(materialIndex, meshIndex));
			}
		}

		em.AddComponentData(singletonEntity,
		                    new ResourceIcons_RenderMeshArray
			                    {Value = new RenderMeshArray(materials.ToArray(), meshes.ToArray())});
	}
}



}
