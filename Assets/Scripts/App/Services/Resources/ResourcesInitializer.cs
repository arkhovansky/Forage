using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using Lib.Grid;

using App.Game.ECS.Components;
using App.Game.ECS.Resource.Plant.Components;



namespace App.Services.Resources {



public class ResourcesInitializer : IResourcesInitializer
{
	private readonly IResourceTypeRepository _resourceTypeRepository;



	public ResourcesInitializer(IResourceTypeRepository resourceTypeRepository)
	{
		_resourceTypeRepository = resourceTypeRepository;
	}



	public void Init(IReadOnlyList<AxialPosition> tilePositions,
	                 IReadOnlyList<uint> resourceTypes,
	                 IReadOnlyList<float> potentialBiomass)
	{
		var world = World.DefaultGameObjectInjectionWorld;
		var entityManager = world.EntityManager;

		var prototype = entityManager.CreateEntity();
		entityManager.AddComponent<TilePosition>(prototype);
		entityManager.AddComponent<PlantResource>(prototype);
		entityManager.AddComponent<RemainingRipeBiomass>(prototype);

		var count = resourceTypes.Count;

		var clonedEntities = new NativeArray<Entity>(count, Allocator.Temp);
		entityManager.Instantiate(prototype, clonedEntities);


		for (int i = 0; i < count; ++i) {
			var entity = clonedEntities[i];

			var resourceTypeId = resourceTypes[i];
			var resourceType = _resourceTypeRepository.Get(resourceTypeId);

			entityManager.SetComponentData(entity, new TilePosition(tilePositions[i]));

			entityManager.SetComponentData(entity, new PlantResource {
				TypeId = resourceTypeId,
				RipenessPeriod = resourceType.RipenessPeriod,
				PotentialBiomass = potentialBiomass[i]
			});

			entityManager.SetComponentData(entity, new RemainingRipeBiomass());
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
}



}
