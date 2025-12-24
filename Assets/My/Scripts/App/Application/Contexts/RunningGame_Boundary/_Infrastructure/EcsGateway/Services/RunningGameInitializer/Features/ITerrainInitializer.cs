using System.Collections.Generic;

using Lib.Grid;

using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features {



public interface ITerrainInitializer
{
	void Init(IReadOnlyList<TerrainTypeId> tileTerrainTypes,
	          RectangularHexMap map,
	          float tilePhysicalInnerDiameter);
}



}
