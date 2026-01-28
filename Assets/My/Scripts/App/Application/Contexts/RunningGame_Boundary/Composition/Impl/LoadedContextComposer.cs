using UnityEngine;

using Lib.AppFlow;
using Lib.Grid;
using Lib.Grid.Spatial;
using Lib.Math;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.Repositories;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.DomainSettings.Repositories;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Locale;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Presentation;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Game.Core;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl;
using App.Application.Contexts.RunningGame_Boundary.Services;
using App.Game.Core;
using App.Infrastructure.EcsGateway.Contracts.Services;
using App.Infrastructure.EcsGateway.Services;
using App.Infrastructure.Shared.Contracts.Database.Presentation;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Application.Contexts.RunningGame_Boundary.Composition.Impl {



public class LoadedContextComposer : ILoadedContextComposer
{
	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;


	private readonly IContextData _contextData;

	//----------------------------------------------------------------------------------------------


	public LoadedContextComposer(IContextData contextData)
	{
		_contextData = contextData;
	}


	//----------------------------------------------------------------------------------------------
	// ILoadedContextComposer


	public void Compose(
		GameDatabase database,
		out ILocaleFactory localeFactory,
		out IRunningGameInitializer runningGameInitializer,
		out IRunningGameInstance runningGame)
	{
		var ecsHelper = new EcsHelper();

		var runningGameInstance = new RunningGameInstance(
			new World_Adapter(
				new Time_Adapter(ecsHelper),
				new Map_Adapter(ecsHelper),
				new PlantResources_Adapter(),
				new Band_Adapter(ecsHelper)),
			_contextData.Get<IEcsSystems_Service>(), ecsHelper);
		runningGame = runningGameInstance;

		var localeRepository = new LocaleAssetRepository(database.Domain.Locales);
		localeFactory = new LocaleFactory(localeRepository);

		var gridLayout = new HexGridLayout_3D(
			new HexGridLayout(HexOrientation),
			new Matrix3x2(Vector3.right, Vector3.forward));

		var terrainType_TextualPresentation_Repository =
			new TerrainType_TextualPresentation_Repository(database.Presentation.TerrainTypes);
		var resourceType_TextualPresentation_Repository =
			new ResourceType_TextualPresentation_Repository(database.Presentation.ResourceTypes);
		var humanType_TextualPresentation_Repository = new HumanType_TextualPresentation_Repository();

		var presentationConfig_Repository = new PresentationConfig_Repository(database.Presentation.Config);

		runningGameInitializer = Create_RunningGameInitializer(
			database, gridLayout, ecsHelper,
			presentationConfig_Repository, resourceType_TextualPresentation_Repository);


		_contextData.Add<IEcsHelper>(ecsHelper);
		_contextData.Add(gridLayout);
		_contextData.Add<IResourceMarker_Config_Repository>(presentationConfig_Repository);
		_contextData.Add<IResourceType_Icon_Repository>(
			new ResourceType_Icon_Repository(database.Presentation.ResourceTypes));
		_contextData.Add<ITerrainType_TextualPresentation_Repository>(terrainType_TextualPresentation_Repository);
		_contextData.Add<IResourceType_TextualPresentation_Repository>(resourceType_TextualPresentation_Repository);
		_contextData.Add<IHumanType_TextualPresentation_Repository>(humanType_TextualPresentation_Repository);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static IRunningGameInitializer Create_RunningGameInitializer(
		GameDatabase database,
		HexGridLayout_3D gridLayout,
		IEcsHelper ecsHelper,
		PresentationConfig_Repository presentationConfig_Repository,
		IResourceType_TextualPresentation_Repository resourceType_TextualPresentation_Repository)
	{
		var mapDataInitializer = new MapDataInitializer(gridLayout, ecsHelper);

		var terrainType_GraphicalPresentation_Repository =
			new TerrainType_GraphicalPresentation_Repository(database.Presentation.TerrainTypes, gridLayout);
		var terrainInitializer =
			new TerrainInitializer(
				gridLayout,
				terrainType_GraphicalPresentation_Repository,
				((IMap_GraphicalPresentation_Repository) presentationConfig_Repository).Get_GridLinesMaterial(),
				ecsHelper);

		var resourceTypeRepository = new ResourceTypeRepository(database.Domain.PlantResourceTypes);
		var resourcesInitializer = new ResourcesInitializer(
			resourceTypeRepository
#if !DOTS_DISABLE_DEBUG_NAMES
			, resourceType_TextualPresentation_Repository
#endif
		);

		var resourceType_GraphicalPresentation_Repository =
			new ResourceType_GraphicalPresentation_Repository(database.Presentation.ResourceTypes);
		var resourcePresentationInitializer =
			new ResourcePresentationInitializer(
				resourceType_GraphicalPresentation_Repository,
				((IPlantResource_PresentationConfig_Repository) presentationConfig_Repository).Get(),
				ecsHelper);

		var gameTimeInitializer = new GameTimeInitializer(ecsHelper);

		var humanTypeRepository = new HumanTypeRepository(database.Domain.HumanTypes);
		var bandInitializer = new BandInitializer(humanTypeRepository);

		var systemParametersRepository = new SystemParametersRepository(database.Domain.SystemParameters);
		var domainSettingsRepository = new DomainSettingsRepository(database.DomainSettings);
		var systemParametersInitializer =
			new SystemsInitializer(systemParametersRepository, domainSettingsRepository);

		return new RunningGameInitializer(
			mapDataInitializer, terrainInitializer, resourcesInitializer, resourcePresentationInitializer,
			gameTimeInitializer, bandInitializer, systemParametersInitializer);
	}
}



}
