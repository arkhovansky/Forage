using System.Collections.Generic;

using Lib.Grid;



namespace App.Services.Terrain {



public interface ITerrainInitializer
{
	void Create(IReadOnlyList<uint> tileTerrainTypes,
	            IReadOnlyList<AxialPosition> tilePositions);
}



}
