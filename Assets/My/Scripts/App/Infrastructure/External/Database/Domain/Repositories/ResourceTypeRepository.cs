using System;
using System.Collections.Generic;
using System.Linq;

using App.Game.Database;
using App.Infrastructure.EcsGateway.Database.Domain;



namespace App.Infrastructure.External.Database.Domain.Repositories {



public class ResourceTypeRepository : IResourceTypeRepository
{
	private readonly Dictionary<ResourceTypeId, PlantResourceType> _resourceTypes = new();


	//----------------------------------------------------------------------------------------------


	public ResourceTypeRepository()
	{
		var typeList = GameDatabase.Instance.Domain.PlantResourceTypes.List;

		foreach (ResourceTypeId typeId in Enum.GetValues(typeof(ResourceTypeId))) {
			_resourceTypes[typeId] = typeList.First(x => x.Id == typeId);
		}
	}


	public PlantResourceType Get(ResourceTypeId resourceTypeId)
	{
		return _resourceTypes[resourceTypeId];
	}
}



}
