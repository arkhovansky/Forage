using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface IResourceTypeRepository
{
	PlantResourceType Get(ResourceTypeId resourceTypeId);
}



}
