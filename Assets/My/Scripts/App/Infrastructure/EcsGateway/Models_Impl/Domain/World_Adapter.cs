using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;



namespace App.Infrastructure.EcsGateway.Models_Impl.Domain {



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
