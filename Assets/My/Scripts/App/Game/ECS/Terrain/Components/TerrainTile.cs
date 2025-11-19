using Unity.Entities;

using App.Game.Database;



namespace App.Game.ECS.Terrain.Components {



public struct TerrainTile : IComponentData
{
	public TerrainTypeId TerrainType;



	public TerrainTile(TerrainTypeId terrainTypeId)
	{
		TerrainType = terrainTypeId;
	}
}



}
