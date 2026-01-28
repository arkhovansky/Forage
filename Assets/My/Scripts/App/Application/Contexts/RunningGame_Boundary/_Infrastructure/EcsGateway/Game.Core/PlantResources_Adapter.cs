using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using App.Game.Core.Query;
using App.Game.ECS.Resource.Plant.Components;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Game.Core {



public class PlantResources_Adapter : IPlantResources
{
	public IReadOnlyList<IPlantResource> Get_RipeResources()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<RipeBiomass>());
		var entities = query.ToEntityArray(Allocator.Temp);

		var list = new List<IPlantResource>(entities.Length);

		foreach (var entity in entities)
			list.Add(new PlantResource_Adapter(entity));

		return list;
	}
}



}
