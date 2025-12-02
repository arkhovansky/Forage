using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Database.Domain {



public interface ITerrainTypeRepository
{
	TerrainType Get(TerrainTypeId terrainTypeId);
}



}
