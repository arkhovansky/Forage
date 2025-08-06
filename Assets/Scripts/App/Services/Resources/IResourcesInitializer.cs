using System.Collections.Generic;

using Lib.Grid;



namespace App.Services.Resources {



public interface IResourcesInitializer
{
	void Init(IReadOnlyList<AxialPosition> mapPositions,
	          IReadOnlyList<uint> resourceTypes,
	          IReadOnlyList<float> potentialBiomass,
	          in RectangularHexMap map);
}



}
