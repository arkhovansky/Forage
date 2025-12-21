using App.Game.Database;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface ITerrainTypeRepository
{
	TerrainType Get(TerrainTypeId terrainTypeId);
}



}
