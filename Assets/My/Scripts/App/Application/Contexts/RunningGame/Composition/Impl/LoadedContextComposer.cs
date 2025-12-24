using UnityEngine;

using Lib.AppFlow;
using Lib.Grid;
using Lib.Grid.Spatial;
using Lib.Math;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Contexts.RunningGame._Infrastructure.Data.Database;
using App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Domain.Repositories;
using App.Application.Contexts.RunningGame._Infrastructure.Data.Database.DomainSettings.Repositories;
using App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Presentation.Repositories;
using App.Application.Contexts.RunningGame._Infrastructure.Data.Locale;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Services;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Game.Core;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services.RunningGameInitializer;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Views;
using App.Application.Contexts.RunningGame._Infrastructure.Shared.Contracts.Database.Presentation;
using App.Application.Contexts.RunningGame._Infrastructure.UI;
using App.Application.Contexts.RunningGame._Infrastructure.UI.ViewModels;
using App.Application.Contexts.RunningGame._Infrastructure.UI.Views;
using App.Application.Contexts.RunningGame.Controller;
using App.Application.Contexts.RunningGame.Models.UI.Impl;
using App.Application.Contexts.RunningGame.Services;
using App.Infrastructure.Shared.Contracts.Services;

using IView = Lib.AppFlow.IView;



namespace App.Application.Contexts.RunningGame.Composition.Impl {



public class LoadedContextComposer : ILoadedContextComposer
{
	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;


	private readonly IEcsSystems_Service _ecsSystems_Service;
	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;

	private HexGridLayout_3D _gridLayout;

	//----------------------------------------------------------------------------------------------


	public LoadedContextComposer(IEcsSystems_Service ecsSystems_Service,
	                             IGui gui,
	                             IVvmBinder vvmBinder)
	{
		_ecsSystems_Service = ecsSystems_Service;
		_gui = gui;
		_vvmBinder = vvmBinder;
	}


	//----------------------------------------------------------------------------------------------
	// ILoadedContextComposer


	public void Compose(
		RunningGameContext context,
		out ILocaleFactory localeFactory,
		out IRunningGameInitializer runningGameInitializer,
		out ILoopComponent runningGame,
		out ILoopComponent uiModel,
		out IController controller,
		out IView worldUI_View,
		out ILoopComponent screenUI_VM,
		out Lib.UICore.Gui.IView screenUI_View)
	{
		var ecsHelper = new EcsHelper();

		var runningGameInstance = new RunningGameInstance(
			new World_Adapter(new Time_Adapter(ecsHelper), new Map_Adapter(ecsHelper), new Band_Adapter(ecsHelper)),
			_ecsSystems_Service, ecsHelper);
		runningGame = runningGameInstance;

		var runningGame_UIModel = new RunningGame_UIModel();
		uiModel = runningGame_UIModel;

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
			_gridLayout, ecsHelper, terrainTypePresentationRepository, resourceTypePresentationRepository);

		var runningGameController = new RunningGameController(runningGameInstance, runningGame_UIModel);
		controller = runningGameController;

		worldUI_View = new WorldUI_View(ecsHelper);

		var runningGame_ScreenUI_VM = new RunningGame_ScreenUI_VM(
			runningGameInstance, runningGame_UIModel,
			terrainTypePresentationRepository, resourceTypePresentationRepository, humanTypePresentationRepository);
		screenUI_VM = runningGame_ScreenUI_VM;
		screenUI_View = new RunningGame_ScreenUI_View(runningGame_ScreenUI_VM,
		                                              _gui, _vvmBinder);

		runningGame_UIModel.Init_PresentationEvent_Emitter(context);
		runningGameController.Init_Command_Emitter(context);
		runningGame_ScreenUI_VM.Init_Command_Emitter(context);

		_gui.AddView(screenUI_View);
	}


	public IView Create_SceneViewController(RectangularHexMap map,
	                                        RunningGameContext context)
	{
		var spatialMap = new Spatial_RectangularHexMap_3D(map, _gridLayout);
		var sceneViewController = new SceneViewController(Camera.main!, spatialMap);
		sceneViewController.Init_InputEvent_Emitter(context);
		return sceneViewController;
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static IRunningGameInitializer Create_RunningGameInitializer(
		HexGridLayout_3D gridLayout,
		IEcsHelper ecsHelper,
		ITerrainTypePresentationRepository terrainTypePresentationRepository,
		IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		var mapDataInitializer = new MapDataInitializer(gridLayout, ecsHelper);

		var mapPresentationRepository = new MapPresentationRepository(GameDatabase.Instance.Presentation);
		var terrainInitializer = new TerrainInitializer(
			gridLayout, terrainTypePresentationRepository, mapPresentationRepository, ecsHelper);

		var resourceTypeRepository = new ResourceTypeRepository(GameDatabase.Instance.Domain.PlantResourceTypes);
		var resourcesInitializer = new ResourcesInitializer(
			resourceTypeRepository
#if !DOTS_DISABLE_DEBUG_NAMES
			, resourceTypePresentationRepository
#endif
		);

		var resourcePresentationInitializer =
			new ResourcePresentationInitializer(resourceTypePresentationRepository, ecsHelper);

		var gameTimeInitializer = new GameTimeInitializer(ecsHelper);

		var humanTypeRepository = new HumanTypeRepository();
		var bandInitializer = new BandInitializer(humanTypeRepository);

		var systemParametersRepository = new SystemParametersRepository(GameDatabase.Instance.Domain.SystemParameters);
		var domainSettingsRepository = new DomainSettingsRepository(GameDatabase.Instance.DomainSettings);
		var systemParametersInitializer =
			new SystemsInitializer(systemParametersRepository, domainSettingsRepository);

		return new RunningGameInitializer(
			mapDataInitializer, terrainInitializer, resourcesInitializer, resourcePresentationInitializer,
			gameTimeInitializer, bandInitializer, systemParametersInitializer);
	}
}



}
