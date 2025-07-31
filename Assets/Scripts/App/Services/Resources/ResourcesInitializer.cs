using System;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using Lib.Grid;

using App.Game.ECS.Map.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Resource.Plant.Presentation.Components;



namespace App.Services.Resources {



public class ResourcesInitializer : IResourcesInitializer
{
	private readonly IResourceTypeRepository _resourceTypeRepository;



	public ResourcesInitializer(IResourceTypeRepository resourceTypeRepository)
	{
		_resourceTypeRepository = resourceTypeRepository;
	}



	public void Init(IReadOnlyList<AxialPosition> mapPositions,
	                 IReadOnlyList<uint> resourceTypes,
	                 IReadOnlyList<float> potentialBiomass)
	{
		if (!(resourceTypes.Count == mapPositions.Count &&
		      potentialBiomass.Count == mapPositions.Count))
			throw new ArgumentException();


		var count = mapPositions.Count;

		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prototype = em.CreateEntity(
			typeof(MapPosition), typeof(PlantResource), typeof(RipeBiomass), typeof(ResourceIcon));

		using (var clonedEntities = new NativeArray<Entity>(count, Allocator.Temp)) {
			em.Instantiate(prototype, clonedEntities);

			for (int i = 0; i < count; ++i) {
				var entity = clonedEntities[i];

				var resourceTypeId = resourceTypes[i];
				var resourceType = _resourceTypeRepository.Get(resourceTypeId);

				em.SetComponentData(entity, new MapPosition(mapPositions[i]));

				em.SetComponentData(entity, new PlantResource {
					TypeId = resourceTypeId,
					RipenessPeriod = resourceType.RipenessPeriod,
					PotentialBiomass = potentialBiomass[i]
				});

				em.SetComponentData(entity, new RipeBiomass());
			}
		}

		em.DestroyEntity(prototype);
	}
}



}
