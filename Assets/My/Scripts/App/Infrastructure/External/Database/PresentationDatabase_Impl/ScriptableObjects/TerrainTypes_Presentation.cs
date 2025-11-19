using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using App.Game.Database;



namespace App.Infrastructure.External.Database.PresentationDatabase_Impl.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(TerrainTypes_Presentation),
                 menuName = AssetMenuNames.Database_GamePresentation_+nameof(TerrainTypes_Presentation))]
public class TerrainTypes_Presentation : ScriptableObject
{
	[Serializable]
	private class TerrainType_Item
	{
		public TerrainTypeId Id;
		public TerrainType_Presentation Data = null!;
	}


	[Serializable]
	public class TerrainType_Presentation
	{
		public Material Material = null!;
	}



	[SerializeField] private List<TerrainType_Item> List = null!;



	public TerrainType_Presentation GetTerrainTypeData(TerrainTypeId id)
	{
		return List.First(x => x.Id == id).Data;
	}
}



}
