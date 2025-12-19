using UnityEngine;

using Lib.AppFlow;
using Lib.Grid;
using Lib.Grid.Spatial;
using Lib.Math;

using App.Application.Flow.GameInstance.RunningGame.Controller;
using App.Application.Flow.GameInstance.RunningGame.Models.UI.Impl;
using App.Application.Services;
using App.Game.Core;
using App.Infrastructure.Common.Contracts.Database.Presentation;
using App.Infrastructure.EcsGateway.Models.Domain;
using App.Infrastructure.EcsGateway.Services;
using App.Infrastructure.EcsGateway.Services.RunningGameInitializer;
using App.Infrastructure.EcsGateway.Views;
using App.Infrastructure.External.Data.Database;
using App.Infrastructure.External.Data.Database.Domain.Repositories;
using App.Infrastructure.External.Data.Database.DomainSettings.Repositories;
using App.Infrastructure.External.Data.Database.Presentation.Repositories;
using App.Infrastructure.External.Data.Locale;
using App.Infrastructure.External.UI.GameInstance.RunningGame;
using App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels;
using App.Infrastructure.External.UI.GameInstance.RunningGame.Views;



namespace App.Application.Flow.GameInstance.RunningGame {



public partial class RunningGameContext
{
	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;


	private IRunningGameInstance _runningGameInstance = null!;

	private HexGridLayout_3D _gridLayout;



	private static IInGameMode Create_InGameMode()
	{
		return new InGameMode();
	}


	private void Compose(
		out ILocaleFactory localeFactory,
		out IRunningGameInitializer runningGameInitializer,
		out IController controller,
		out IView worldUI_View)
	{
		var runningGame = new RunningGameInstance(
			new World_Adapter(new Time_Adapter(), new Map_Adapter(), new Band_Adapter()));
		_runningGame = runningGame;
		_runningGameInstance = runningGame;

		var uiModel = new RunningGame_UIModel(this);
		_uiModel = uiModel;

		var localeRepository = new LocaleAssetRepository(GameDatabase.Instance.Domain.Locales);
		localeFactory = new LocaleFactory(localeRepository);

		_gridLayout = new HexGridLayout_3D(
			new HexGridLayout(HexOrientation),
			new Matrix3x2(Vector3.right, Vector3.forward));

		var terrainTypePresentationRepository =
			new TerrainTypePresentationRepository(GameDatabase.Instance.Presentation.TerrainTypes, _gridLayout);
		var resourceTypePresentationRepository =
			new ResourceTypePresentationRepository(GameDatabase.Instance.Presentation.ResourceTypes);
		var humanTypePresentationRepository = new HumanTypePresentationRepository();

		runningGameInitializer = Create_RunningGameInitializer(
			_gridLayout, terrainTypePresentationRepository, resourceTypePresentationRepository);

		controller = new RunningGameController(_runningGameInstance, uiModel, this);

		worldUI_View = new WorldUI_View();

		var screenUI_VM = new RunningGame_ScreenUI_VM(
			runningGame, uiModel,
			this,
			terrainTypePresentationRepository, resourceTypePresentationRepository, humanTypePresentationRepository);
		_screenUI_VM = screenUI_VM;
		_screenUI_View = new RunningGame_ScreenUI_View(screenUI_VM,
		                                               _gui, _vvmBinder);
		_gui.AddView(_screenUI_View);
	}


	private static IRunningGameInitializer Create_RunningGameInitializer(
		HexGridLayout_3D gridLayout,
		ITerrainTypePresentationRepository terrainTypePresentationRepository,
		IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		var terrainInitializer = new TerrainInitializer(gridLayout, terrainTypePresentationRepository);

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
			bandInitializer, systemParametersInitializer, gridLayout);
	}


	private SceneViewController Create_SceneViewController(RectangularHexMap map)
	{
		var spatialMap = new Spatial_RectangularHexMap_3D(map, _gridLayout);
		return new SceneViewController(Camera.main!, spatialMap, this);
	}
}



}
