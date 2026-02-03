using System;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using Lib.Grid;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.Database;
using App.Game.ECS.Map;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Resource.Plant.Presentation.Components;
#if !DOTS_DISABLE_DEBUG_NAMES
using App.Infrastructure.Shared.Contracts.Database.Presentation;
#endif



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



public class ResourcesInitializer : IResourcesInitializer
{
	private readonly IResourceTypeRepository _resourceTypeRepository;

#if !DOTS_DISABLE_DEBUG_NAMES
	private readonly IResourceType_TextualPresentation_Repository _resourceTypePresentationRepository;
#endif

	//----------------------------------------------------------------------------------------------


	public ResourcesInitializer(
		IResourceTypeRepository resourceTypeRepository
#if !DOTS_DISABLE_DEBUG_NAMES
		, IResourceType_TextualPresentation_Repository resourceTypePresentationRepository
#endif
	)
	{
		_resourceTypeRepository = resourceTypeRepository;
#if !DOTS_DISABLE_DEBUG_NAMES
		_resourceTypePresentationRepository = resourceTypePresentationRepository;
#endif
	}


	//----------------------------------------------------------------------------------------------
	// IResourcesInitializer


	public void Init(IReadOnlyList<AxialPosition> mapPositions,
	                 IReadOnlyList<ResourceTypeId> resourceTypes,
	                 IReadOnlyList<float> potentialBiomass,
	                 in RectangularHexMap map)
	{
		if (!(resourceTypes.Count == mapPositions.Count &&
		      potentialBiomass.Count == mapPositions.Count))
			throw new ArgumentException();


		var count = mapPositions.Count;

		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		var ecsMap = CreateEcsMap(map);

		var prototype = em.CreateEntity(
			typeof(MapPosition), typeof(PlantResource), typeof(ResourceIcon));

		using (var clonedEntities = new NativeArray<Entity>(count, Allocator.Temp)) {
			em.Instantiate(prototype, clonedEntities);

			for (int i = 0; i < count; ++i) {
				var entity = clonedEntities[i];

				var mapPosition = mapPositions[i];

				var resourceTypeId = resourceTypes[i];
				var resourceType = _resourceTypeRepository.Get(resourceTypeId);

				em.SetComponentData(entity, new MapPosition(mapPosition));

				em.SetComponentData(entity, new PlantResource {
					TypeId = resourceTypeId,
					RipenessPeriod = resourceType.RipenessPeriod,
					PotentialBiomass = potentialBiomass[i]
				});

#if !DOTS_DISABLE_DEBUG_NAMES
				var name = _resourceTypePresentationRepository.GetName(resourceTypeId);
				em.SetName(entity, $"Resource: {name} {mapPosition}");
#endif

				Set_TilePlantResource(ecsMap.GetTileEntity(mapPosition), entity);
			}
		}

		em.DestroyEntity(prototype);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private EcsMap CreateEcsMap(in RectangularHexMap map)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		var tileBuffer =
			em.CreateEntityQuery(typeof(MapTileEntity)).GetSingletonBuffer<MapTileEntity>(isReadOnly: true);
		return new EcsMap(map, tileBuffer);
	}


	private void Set_TilePlantResource(Entity tileEntity, Entity resourceEntity)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.SetComponentData(tileEntity, new TilePlantResource(resourceEntity));
	}
}



}
