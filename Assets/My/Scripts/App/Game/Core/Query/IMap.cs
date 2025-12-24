using Lib.Grid;

using App.Game.Database;



namespace App.Game.Core.Query {



public interface IMap
{
	RectangularHexMap Get_GridMap();

	TerrainTypeId Get_TerrainTypeId(AxialPosition tile);

	IPlantResource? Get_PlantResource(AxialPosition tile);
}



}
