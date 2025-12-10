using App.Game.Core.Query;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class World_Adapter : IWorld_RO
{
	public ITime Time { get; }

	public IMap Map { get; }

	public IBand_RO Band { get; }



	public World_Adapter(ITime time, IMap map, IBand_RO band)
	{
		Time = time;
		Map = map;
		Band = band;
	}
}



}
