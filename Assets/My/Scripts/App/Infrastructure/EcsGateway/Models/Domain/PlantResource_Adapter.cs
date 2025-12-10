using Unity.Entities;

using App.Game.Core.Query;
using App.Game.ECS.Resource.Plant.Components;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class PlantResource_Adapter : IPlantResource
{
	private readonly Entity _entity;

	private EntityManager _entityManager
		= World.DefaultGameObjectInjectionWorld.EntityManager;



	public PlantResource_Adapter(Entity entity)
	{
		_entity = entity;
	}


	public PlantResource Get_StaticData()
	{
		return _entityManager.GetComponentData<PlantResource>(_entity);
	}


	public float Get_RipeBiomass()
	{
		return _entityManager.GetComponentData<RipeBiomass>(_entity).Value;
	}
}



}
