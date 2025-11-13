using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface IMap
{
	uint Get_TerrainTypeId(AxialPosition tile);

	IPlantResource? Get_PlantResource(AxialPosition tile);
}



}
