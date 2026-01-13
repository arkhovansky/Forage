using System;
using System.Collections.Generic;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Game.Database;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class TerrainType_TextualPresentation_Repository : ITerrainType_TextualPresentation_Repository
{
	private readonly Dictionary<TerrainTypeId, string> _names = new();

	//----------------------------------------------------------------------------------------------


	public TerrainType_TextualPresentation_Repository(TerrainTypes_Presentation asset)
	{
		foreach (TerrainTypeId typeId in Enum.GetValues(typeof(TerrainTypeId)))
			_names[typeId] = asset.GetTerrainTypeData(typeId).Name;
	}


	public string GetName(TerrainTypeId typeId)
	{
		return _names[typeId];
	}
}



}
