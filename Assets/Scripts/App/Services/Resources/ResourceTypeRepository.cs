using System.Collections.Generic;

using App.Game;



namespace App.Services.Resources {



public class ResourceTypeRepository : IResourceTypeRepository
{
	private readonly Dictionary<uint, ResourceType> _resourceTypes = new();



	//----------------------------------------------------------------------------------------------
	// public

	public ResourceTypeRepository()
	{
		_resourceTypes[0] = new ResourceType("Acorns", new YearPeriod(Month.September));
		_resourceTypes[1] = new ResourceType("Bananas", new YearPeriod(Month.June));
		_resourceTypes[2] = new ResourceType("Wheat", new YearPeriod(Month.August));
		_resourceTypes[3] = new ResourceType("Yam", new YearPeriod(Month.July));
		// _resourceTypes[4] = new ResourceType("Apples", new YearPeriod(Month.June));
		// _resourceTypes[5] = new ResourceType("Oranges", new YearPeriod(Month.December));
	}


	public ResourceType Get(uint resourceTypeId)
	{
		return _resourceTypes[resourceTypeId];
	}
}



}
