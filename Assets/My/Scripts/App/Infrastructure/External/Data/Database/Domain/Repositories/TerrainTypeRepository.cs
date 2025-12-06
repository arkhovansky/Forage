using System;
using System.Collections.Generic;
using System.Linq;

using App.Game.Database;
using App.Infrastructure.EcsGateway.Database.Domain;



namespace App.Infrastructure.External.Data.Database.Domain.Repositories {



public class TerrainTypeRepository : ITerrainTypeRepository
{
	private readonly Dictionary<TerrainTypeId, TerrainType> _terrainTypes = new();


	//----------------------------------------------------------------------------------------------


	public TerrainTypeRepository()
	{
		var typeList = GameDatabase.Instance.Domain.TerrainTypes.List;

		foreach (TerrainTypeId typeId in Enum.GetValues(typeof(TerrainTypeId))) {
			_terrainTypes[typeId] = typeList.First(x => x.Id == typeId);
		}
	}


	public TerrainType Get(TerrainTypeId terrainTypeId)
	{
		return _terrainTypes[terrainTypeId];
	}
}



}
