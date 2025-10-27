using System.Collections.Generic;



namespace App.Services.Terrain {



public class TerrainTypeRepository : ITerrainTypeRepository
{
	private readonly Dictionary<uint, TerrainType> _terrainTypes = new();


	//----------------------------------------------------------------------------------------------


	public TerrainTypeRepository()
	{
		_terrainTypes[0] = new TerrainType("Fresh water");
		_terrainTypes[1] = new TerrainType("Sea");
		_terrainTypes[2] = new TerrainType("Ocean");
		_terrainTypes[3] = new TerrainType("Grasslands");
		_terrainTypes[4] = new TerrainType("Plains");
		_terrainTypes[5] = new TerrainType("Forest");
		_terrainTypes[6] = new TerrainType("Tropical forest");
		_terrainTypes[7] = new TerrainType("Swampy tropical forest");
		_terrainTypes[8] = new TerrainType("Hills");
		_terrainTypes[9] = new TerrainType("Mountains");
	}


	public TerrainType Get(uint terrainTypeId)
	{
		return _terrainTypes[terrainTypeId];
	}
}



}
