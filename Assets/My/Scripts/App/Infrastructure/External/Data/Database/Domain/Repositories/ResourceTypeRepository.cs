using System;
using System.Collections.Generic;
using System.Linq;

using App.Game.Database;
using App.Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Infrastructure.External.Data.Database.Domain.ScriptableObjects;



namespace App.Infrastructure.External.Data.Database.Domain.Repositories {



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
