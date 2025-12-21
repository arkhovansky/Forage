using System.Collections.Generic;

using App.Game.Database;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features {



public interface IResourcePresentationInitializer
{
	void Init(ISet<ResourceTypeId> resourceTypeIds);
}



}
