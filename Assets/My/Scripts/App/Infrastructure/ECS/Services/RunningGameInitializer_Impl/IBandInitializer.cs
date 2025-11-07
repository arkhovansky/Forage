using System.Collections.Generic;



namespace App.Infrastructure.ECS.Services.RunningGameInitializer_Impl {



public interface IBandInitializer
{
	void Init(IDictionary<uint, uint> bandMemberTypeCounts);
}



}
