using System.Collections.Generic;

using Lib.Grid;



namespace App.Services.Terrain {



public interface ITerrainInitializer
{
	void Init(IReadOnlyList<uint> tileTerrainTypes,
	          RectangularHexMap map);
}



}
