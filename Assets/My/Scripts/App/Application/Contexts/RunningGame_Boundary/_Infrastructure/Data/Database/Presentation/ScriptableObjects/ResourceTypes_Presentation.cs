using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using App.Game.Database;
using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(ResourceTypes_Presentation),
                 menuName = AssetMenuNames.Root_Database_GamePresentation_+nameof(ResourceTypes_Presentation),
                 order = 2)]
public class ResourceTypes_Presentation : ScriptableObject
{
	[Serializable]
	private class ResourceType_Item
	{
		public ResourceTypeId Id;
		public ResourceType_Presentation Data = null!;
	}


	[Serializable]
	public class ResourceType_Presentation
	{
		public string Name = null!;
		public Material Material = null!;
		public Texture2D IconTexture = null!;
	}



	[SerializeField] private List<ResourceType_Item> List = null!;



	public ResourceType_Presentation GetResourceTypeData(ResourceTypeId id)
	{
		return List.First(x => x.Id == id).Data;
	}
}



}
