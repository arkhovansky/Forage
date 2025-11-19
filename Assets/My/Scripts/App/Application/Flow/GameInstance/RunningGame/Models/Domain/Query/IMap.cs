using Lib.Grid;

using App.Game.Database;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface IMap
{
	TerrainTypeId Get_TerrainTypeId(AxialPosition tile);

	IPlantResource? Get_PlantResource(AxialPosition tile);
}



}
