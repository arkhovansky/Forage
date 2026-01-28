using System;
using System.Collections.Generic;

using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Game.Database;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class ResourceType_Icon_Repository : IResourceType_Icon_Repository
{
	private readonly Dictionary<ResourceTypeId, Texture2D> _icons = new();

	//----------------------------------------------------------------------------------------------


	public ResourceType_Icon_Repository(ResourceTypes_Presentation asset)
	{
		foreach (ResourceTypeId typeId in Enum.GetValues(typeof(ResourceTypeId)))
			_icons[typeId] = asset.GetResourceTypeData(typeId).IconTexture;
	}


	public Texture2D Get(ResourceTypeId typeId)
	{
		return _icons[typeId];
	}
}



}
