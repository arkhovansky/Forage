namespace App.Services.Terrain {



public class TerrainType
{
	// public uint Id;

	public string Name;


	public TerrainType(string name)
	{
		Name = name;
	}
}



public interface ITerrainTypeRepository
{
	TerrainType Get(uint terrainTypeId);
}



}
