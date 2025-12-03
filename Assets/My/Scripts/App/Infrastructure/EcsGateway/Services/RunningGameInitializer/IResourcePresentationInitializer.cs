using System.Collections.Generic;

using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer {



public interface IResourcePresentationInitializer
{
	void Init(ISet<ResourceTypeId> resourceTypeIds);
}



}
