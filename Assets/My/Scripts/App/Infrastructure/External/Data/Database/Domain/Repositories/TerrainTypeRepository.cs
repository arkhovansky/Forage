using System;
using System.Collections.Generic;
using System.Linq;

using App.Game.Database;
using App.Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Infrastructure.External.Data.Database.Domain.ScriptableObjects;



namespace App.Infrastructure.External.Data.Database.Domain.Repositories {



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
