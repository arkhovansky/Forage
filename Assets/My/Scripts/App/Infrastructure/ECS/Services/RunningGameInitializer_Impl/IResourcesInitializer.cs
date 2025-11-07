using System.Collections.Generic;

using Lib.Grid;



namespace App.Infrastructure.ECS.Services.RunningGameInitializer_Impl {



public interface IResourcesInitializer
{
	void Init(IReadOnlyList<AxialPosition> mapPositions,
	          IReadOnlyList<uint> resourceTypes,
	          IReadOnlyList<float> potentialBiomass,
	          in RectangularHexMap map);
}



}
