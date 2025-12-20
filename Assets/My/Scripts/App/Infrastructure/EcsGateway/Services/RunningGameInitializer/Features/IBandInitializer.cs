using System.Collections.Generic;

using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer.Features {



public interface IBandInitializer
{
	void Init(IDictionary<HumanTypeId, uint> humanTypeCounts);
}



}
