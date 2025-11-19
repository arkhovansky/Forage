using System.Collections.Generic;

using Lib.Grid;

using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public interface IResourcesInitializer
{
	void Init(IReadOnlyList<AxialPosition> mapPositions,
	          IReadOnlyList<ResourceTypeId> resourceTypes,
	          IReadOnlyList<float> potentialBiomass,
	          in RectangularHexMap map);
}



}
