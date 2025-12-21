using System.Collections.Generic;

using Lib.Grid;

using App.Game.Database;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features {



public interface IResourcesInitializer
{
	void Init(IReadOnlyList<AxialPosition> mapPositions,
	          IReadOnlyList<ResourceTypeId> resourceTypes,
	          IReadOnlyList<float> potentialBiomass,
	          in RectangularHexMap map);
}



}
