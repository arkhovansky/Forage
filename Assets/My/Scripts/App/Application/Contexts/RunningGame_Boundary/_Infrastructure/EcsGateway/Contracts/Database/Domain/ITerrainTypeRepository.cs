using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface ITerrainTypeRepository
{
	TerrainType Get(TerrainTypeId terrainTypeId);
}



}
