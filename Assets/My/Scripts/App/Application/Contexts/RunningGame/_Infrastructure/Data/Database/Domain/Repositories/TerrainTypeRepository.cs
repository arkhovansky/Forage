using System;
using System.Collections.Generic;
using System.Linq;

using App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Domain.ScriptableObjects;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.Database;



namespace App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Domain.Repositories {



public class TerrainTypeRepository : ITerrainTypeRepository
{
	private readonly Dictionary<TerrainTypeId, TerrainType> _terrainTypes = new();


	//----------------------------------------------------------------------------------------------


	public TerrainTypeRepository(TerrainTypes terrainTypes_Asset)
	{
		foreach (TerrainTypeId typeId in Enum.GetValues(typeof(TerrainTypeId))) {
			_terrainTypes[typeId] = terrainTypes_Asset.List.First(x => x.Id == typeId);
		}
	}


	public TerrainType Get(TerrainTypeId terrainTypeId)
	{
		return _terrainTypes[terrainTypeId];
	}
}



}
