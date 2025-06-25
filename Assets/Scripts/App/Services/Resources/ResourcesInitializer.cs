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
		var world = World.DefaultGameObjectInjectionWorld;
		var entityManager = world.EntityManager;

		var prototype = entityManager.CreateEntity(
			typeof(MapPosition), typeof(PlantResource), typeof(RipeBiomass), typeof(ResourceIcon));

		var count = resourceTypes.Count;

		var clonedEntities = new NativeArray<Entity>(count, Allocator.Temp);
		entityManager.Instantiate(prototype, clonedEntities);

		for (int i = 0; i < count; ++i) {
			var entity = clonedEntities[i];

			var resourceTypeId = resourceTypes[i];
			var resourceType = _resourceTypeRepository.Get(resourceTypeId);

			entityManager.SetComponentData(entity, new MapPosition(mapPositions[i]));

			entityManager.SetComponentData(entity, new PlantResource {
				TypeId = resourceTypeId,
				RipenessPeriod = resourceType.RipenessPeriod,
				PotentialBiomass = potentialBiomass[i]
			});

			entityManager.SetComponentData(entity, new RipeBiomass());
		}

		clonedEntities.Dispose();

		entityManager.DestroyEntity(prototype);
	}
}



}
