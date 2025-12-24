using System;
using System.Collections.Generic;
using System.Linq;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.Repositories {



public class ResourceTypeRepository : IResourceTypeRepository
{
	private readonly Dictionary<ResourceTypeId, PlantResourceType> _resourceTypes = new();


	//----------------------------------------------------------------------------------------------


	public ResourceTypeRepository(PlantResourceTypes resourceTypes_Asset)
	{
		foreach (ResourceTypeId typeId in Enum.GetValues(typeof(ResourceTypeId))) {
			_resourceTypes[typeId] = resourceTypes_Asset.List.First(x => x.Id == typeId);
		}
	}


	public PlantResourceType Get(ResourceTypeId resourceTypeId)
	{
		return _resourceTypes[resourceTypeId];
	}
}



}
