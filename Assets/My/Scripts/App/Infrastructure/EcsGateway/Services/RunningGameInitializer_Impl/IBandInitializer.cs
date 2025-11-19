using System.Collections.Generic;

using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public interface IBandInitializer
{
	void Init(IDictionary<HumanTypeId, uint> humanTypeCounts);
}



}
