using System.Collections.Generic;

using Lib.Grid;



namespace App.Infrastructure.ECS.Services.RunningGameInitializer_Impl {



public interface ITerrainInitializer
{
	void Init(IReadOnlyList<uint> tileTerrainTypes,
	          RectangularHexMap map,
	          float tilePhysicalInnerDiameter);
}



}
