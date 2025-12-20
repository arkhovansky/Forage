using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface IResourceTypeRepository
{
	PlantResourceType Get(ResourceTypeId resourceTypeId);
}



}
