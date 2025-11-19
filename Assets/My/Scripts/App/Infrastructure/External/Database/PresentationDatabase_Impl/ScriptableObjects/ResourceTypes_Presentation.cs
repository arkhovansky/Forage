using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using App.Game.Database;



namespace App.Infrastructure.External.Database.PresentationDatabase_Impl.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(ResourceTypes_Presentation),
                 menuName = AssetMenuNames.Database_GamePresentation_+nameof(ResourceTypes_Presentation))]
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
		public Material Material = null!;
	}



	[SerializeField] private List<ResourceType_Item> List = null!;



	public ResourceType_Presentation GetResourceTypeData(ResourceTypeId id)
	{
		return List.First(x => x.Id == id).Data;
	}
}



}
