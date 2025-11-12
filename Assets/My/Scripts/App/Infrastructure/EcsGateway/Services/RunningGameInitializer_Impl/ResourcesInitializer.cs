using System;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using Lib.Grid;

using App.Game.Database;
using App.Game.ECS.Map;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Resource.Plant.Presentation.Components;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public class ResourcesInitializer : IResourcesInitializer
{
	private readonly IResourceTypeRepository _resourceTypeRepository;



	public ResourcesInitializer(IResourceTypeRepository resourceTypeRepository)
	{
		_resourceTypeRepository = resourceTypeRepository;
	}



	public void Init(IReadOnlyList<AxialPosition> mapPositions,
	                 IReadOnlyList<uint> resourceTypes,
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
			typeof(MapPosition), typeof(PlantResource), typeof(RipeBiomass), typeof(ResourceIcon));

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

				em.SetComponentData(entity, new RipeBiomass());

				em.SetName(entity, $"Resource: {resourceType.Name} {mapPosition}");

				Set_TilePlantResource(ecsMap.GetTileEntity(mapPosition), entity);
			}
		}

		em.DestroyEntity(prototype);
	}



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
