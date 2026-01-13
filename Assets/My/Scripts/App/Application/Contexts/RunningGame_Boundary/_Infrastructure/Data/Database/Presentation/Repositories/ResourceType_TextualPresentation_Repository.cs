using System;
using System.Collections.Generic;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Game.Database;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class ResourceType_TextualPresentation_Repository : IResourceType_TextualPresentation_Repository
{
	private readonly Dictionary<ResourceTypeId, string> _names = new();

	//----------------------------------------------------------------------------------------------


	public ResourceType_TextualPresentation_Repository(ResourceTypes_Presentation asset)
	{
		foreach (ResourceTypeId typeId in Enum.GetValues(typeof(ResourceTypeId)))
			_names[typeId] = asset.GetResourceTypeData(typeId).Name;
	}


	public string GetName(ResourceTypeId typeId)
	{
		return _names[typeId];
	}
}



}
