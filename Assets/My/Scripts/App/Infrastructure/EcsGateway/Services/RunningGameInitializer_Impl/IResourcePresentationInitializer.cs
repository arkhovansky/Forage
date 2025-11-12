using System.Collections.Generic;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public interface IResourcePresentationInitializer
{
	void Init(ISet<uint> resourceTypeIds);
}



}
