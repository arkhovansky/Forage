using Unity.Entities;



namespace App.Game.ECS.Terrain.Components {



public struct TerrainTile : IComponentData
{
	public uint TerrainType;



	public TerrainTile(uint terrainTypeId)
	{
		TerrainType = terrainTypeId;
	}
}



}
