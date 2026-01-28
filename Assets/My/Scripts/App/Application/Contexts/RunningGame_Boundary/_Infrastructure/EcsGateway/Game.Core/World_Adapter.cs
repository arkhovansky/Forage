using App.Game.Core;
using App.Game.Core.Query;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Game.Core {



public class World_Adapter : IWorld
{
	public World_Adapter(ITime time, IMap map, IPlantResources plantResources, IBand band)
	{
		Time = time;
		Map = map;
		PlantResources = plantResources;
		Band = band;
	}


	//----------------------------------------------------------------------------------------------
	// IWorld_RO implementation


	public ITime Time { get; }

	public IMap Map { get; }

	IBand_RO IWorld_RO.Band => Band;


	//----------------------------------------------------------------------------------------------
	// IWorld implementation


	public IPlantResources PlantResources { get; }

	public IBand Band { get; }
}



}
