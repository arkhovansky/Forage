using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Presentation;
using App.Game.ECS.Resource.Plant.Presentation.Components.Config;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class PresentationConfig_Repository
	: IPlantResource_PresentationConfig_Repository
{
	private readonly PresentationConfig _asset;



	public PresentationConfig_Repository(PresentationConfig asset)
	{
		_asset = asset;
	}


	PlantResourcePresentation_Config IPlantResource_PresentationConfig_Repository.Get()
	{
		return new PlantResourcePresentation_Config(
			_asset.PlantResourceIcons.BiomassPerIcon,
			_asset.PlantResourceIcons.RelativeIconSize);
	}
}



}
