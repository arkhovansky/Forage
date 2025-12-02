using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Database.Domain {



public interface IResourceTypeRepository
{
	PlantResourceType Get(ResourceTypeId resourceTypeId);
}



}
