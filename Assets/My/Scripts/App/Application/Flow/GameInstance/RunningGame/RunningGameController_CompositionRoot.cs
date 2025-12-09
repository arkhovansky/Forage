using UnityEngine;

using Lib.Grid;
using Lib.VisualGrid;
using Lib.Math;

using App.Application.Database.Presentation;
using App.Application.Flow.GameInstance.RunningGame.ViewModels;
using App.Application.Services;
using App.Infrastructure.EcsGateway.Models.Domain;
using App.Infrastructure.EcsGateway.Models.Presentation;
using App.Infrastructure.EcsGateway.Services;
using App.Infrastructure.EcsGateway.Services.RunningGameInitializer;
using App.Infrastructure.External.Data.Database;
using App.Infrastructure.External.Data.Database.Domain.Repositories;
using App.Infrastructure.External.Data.Database.DomainSettings.Repositories;
using App.Infrastructure.External.Data.Database.Presentation.Repositories;
using App.Infrastructure.External.Data.Locale;



namespace App.Application.Flow.GameInstance.RunningGame {



public partial class RunningGameController
{
	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;


	private HexLayout3D _hexLayout;



	private static IInGameMode Create_InGameMode()
	{
		return new InGameMode();
	}


	private void Compose(
		out ILocaleFactory localeFactory,
		out IRunningGameInitializer runningGameInitializer)
	{
		_runningGame = new RunningGameInstance(
			new World_Adapter(new Time_Adapter(), new Map_Adapter(), new Band_Adapter()));

		_scenePresentationModel = new ScenePresentationModel();

		var localeRepository = new LocaleAssetRepository(GameDatabase.Instance.Domain.Locales);
		localeFactory = new LocaleFactory(localeRepository);

		_hexLayout = new HexLayout3D(
			new HexLayout(HexOrientation),
			new Matrix3x2(Vector3.right, Vector3.forward));

		var terrainTypePresentationRepository =
			new TerrainTypePresentationRepository(GameDatabase.Instance.Presentation.TerrainTypes, _hexLayout);
		var resourceTypePresentationRepository =
			new ResourceTypePresentationRepository(GameDatabase.Instance.Presentation.ResourceTypes);
		var humanTypePresentationRepository = new HumanTypePresentationRepository();

		runningGameInitializer = Create_RunningGameInitializer(
			_hexLayout, terrainTypePresentationRepository, resourceTypePresentationRepository);

		_uiVM = new RunningGameUI_VM(
			_runningGame, _scenePresentationModel, this,
			CommandRouter,
			terrainTypePresentationRepository, resourceTypePresentationRepository, humanTypePresentationRepository);
		_uiView = new RunningGameUI_View(_uiVM,
		                                 _gui, _vvmBinder);
		_gui.AddView(_uiView);

		// Should come at the end of composition root since modes might use data members of this
		_arrival_Mode = new Arrival_Mode(this);
		_campPlacing_Mode = new CampPlacing_Mode(this);
		_periodRunning_Mode = new PeriodRunning_Mode(this);
		_interPeriod_Mode = new InterPeriod_Mode(this);
	}


	private static IRunningGameInitializer Create_RunningGameInitializer(
		HexLayout3D hexLayout,
		ITerrainTypePresentationRepository terrainTypePresentationRepository,
		IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		var terrainInitializer = new TerrainInitializer(hexLayout, terrainTypePresentationRepository);

		var resourceTypeRepository = new ResourceTypeRepository(GameDatabase.Instance.Domain.PlantResourceTypes);
		var resourcesInitializer = new ResourcesInitializer(
			resourceTypeRepository
#if !DOTS_DISABLE_DEBUG_NAMES
			, resourceTypePresentationRepository
#endif
		);

		var resourcePresentationInitializer = new ResourcePresentationInitializer(resourceTypePresentationRepository);

		var gameTimeInitializer = new GameTimeInitializer();

		var humanTypeRepository = new HumanTypeRepository();
		var bandInitializer = new BandInitializer(humanTypeRepository);

		var systemParametersRepository = new SystemParametersRepository(GameDatabase.Instance.Domain.SystemParameters);
		var domainSettingsRepository = new DomainSettingsRepository(GameDatabase.Instance.DomainSettings);
		var systemParametersInitializer =
			new SystemsInitializer(systemParametersRepository, domainSettingsRepository);

		return new RunningGameInitializer(
			terrainInitializer, resourcesInitializer, resourcePresentationInitializer, gameTimeInitializer,
			bandInitializer, systemParametersInitializer, hexLayout);
	}


	private SceneViewController Create_SceneViewController(RectangularHexMap map)
	{
		var visualMap = new VisualRectangularHexMap3D(map, _hexLayout);
		return new SceneViewController(Camera.main!, visualMap,
		                               CommandRouter);
	}
}



}
