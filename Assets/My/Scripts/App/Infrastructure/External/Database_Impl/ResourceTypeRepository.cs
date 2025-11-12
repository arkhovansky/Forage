using System.Collections.Generic;

using App.Game.Database;
using App.Game.ECS.GameTime.Components;



namespace App.Infrastructure.External.Database_Impl {



public class ResourceTypeRepository : IResourceTypeRepository
{
	private readonly Dictionary<uint, ResourceType> _resourceTypes = new();



	//----------------------------------------------------------------------------------------------
	// public

	public ResourceTypeRepository()
	{
		_resourceTypes[0] = new ResourceType("Yam", new YearPeriod(Month.January));
		// _resourceTypes[0] = new ResourceType("Acorns", new YearPeriod(Month.September));
		// _resourceTypes[1] = new ResourceType("Bananas", new YearPeriod(Month.June));
		// _resourceTypes[2] = new ResourceType("Wheat", new YearPeriod(Month.August));
	}


	public ResourceType Get(uint resourceTypeId)
	{
		return _resourceTypes[resourceTypeId];
	}
}



}
