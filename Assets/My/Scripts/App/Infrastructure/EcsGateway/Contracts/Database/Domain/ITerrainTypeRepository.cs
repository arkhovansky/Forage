using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface ITerrainTypeRepository
{
	TerrainType Get(TerrainTypeId terrainTypeId);
}



}
