using System.Collections.Generic;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public interface IBandInitializer
{
	void Init(IDictionary<uint, uint> humanTypeCounts);
}



}
