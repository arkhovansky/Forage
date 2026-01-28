using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Presentation;
using App.Game.ECS.Resource.Plant.Presentation.Components.Config;
using App.Infrastructure.Shared.Contracts.Database.Presentation;
using App.Infrastructure.Shared.Contracts.Database.Presentation.Types;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class PresentationConfig_Repository
	: IMap_GraphicalPresentation_Repository,
	  IPlantResource_PresentationConfig_Repository,
	  // Shared
	  IResourceMarker_Config_Repository
{
	private readonly PresentationConfig _asset;

	//----------------------------------------------------------------------------------------------


	public PresentationConfig_Repository(PresentationConfig asset)
	{
		_asset = asset;
	}


	//----------------------------------------------------------------------------------------------


	Material IMap_GraphicalPresentation_Repository.Get_GridLinesMaterial()
		=> _asset.TerrainGridLinesMaterial;


	//----------------------------------------------------------------------------------------------


	PlantResourcePresentation_Config IPlantResource_PresentationConfig_Repository.Get()
	{
		return new PlantResourcePresentation_Config(
			_asset.PlantResourceIcons.BiomassPerIcon,
			_asset.PlantResourceIcons.RelativeIconSize);
	}


	//----------------------------------------------------------------------------------------------


	GameObject IResourceMarker_Config_Repository.Prefab
		=> _asset.ResourceMarkers.Prefab;

	ResourceMarker_Parameters IResourceMarker_Config_Repository.Parameters {
		get {
			var assetData = _asset.ResourceMarkers.Parameters;
			return new ResourceMarker_Parameters(
				assetData.AreaCoefficient, assetData.MinCoreRadius, assetData.BorderToCoreRatio);
		}
	}
}



}
