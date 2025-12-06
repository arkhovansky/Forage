using UnityEngine;

using Cysharp.Threading.Tasks;

using Lib.Grid;
using Lib.VisualGrid;
using Lib.Math;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Flow.Impl;
using App.Application.Framework.UICore.Gui;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Database.Presentation;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Application.Flow.GameInstance.RunningGame.ViewModels;
using App.Application.Services;
using App.Game.Meta;
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



public partial class RunningGameController : Controller
{
	private interface IMode
	{
		void Enter() {}
		void Exit() {}

		void Update() {}
	}


	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;


	private readonly IGameInstance _game;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;


	#region Semantically readonly

	private IInGameMode _inGameMode = null!;

	private IRunningGameInstance _runningGame = null!;

	private IScenePresentationModel _scenePresentationModel = null!;

	private RunningGameUI_VM _uiVM = null!;
	private RunningGameUI_View _uiView = null!;

	private IMode _arrival_Mode = null!;
	private IMode _campPlacing_Mode = null!;
	private IMode _periodRunning_Mode = null!;
	private IMode _interPeriod_Mode = null!;

	#endregion


	private IMode _mode = null!;


	//----------------------------------------------------------------------------------------------


	public RunningGameController(IGameInstance game,
	                             IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_game = game;

		_gui = gui;
		_vvmBinder = vvmBinder;

		base.AddCommandHandler<EnterPlaceCampMode>(OnEnterPlaceCampMode);
		base.AddCommandHandler<PlaceCamp>(OnPlaceCamp);
		base.AddCommandHandler<RunYearPeriod>(OnRunYearPeriod);
		base.AddCommandHandler<YearPeriodChanged>(OnYearPeriodChanged);
		base.AddCommandHandler<HoveredTileChanged>(OnHoveredTileChanged);
	}


	public override async UniTask Start()
	{
		_inGameMode = new InGameMode();

		await _inGameMode.Enter();
		// GameDatabase.Instance is available now

		Compose(out var localeFactory,
		        out var runningGameInitializer,
		        out var hexLayout);

		var locale = localeFactory.Create(_game.LocaleId);

		runningGameInitializer.Initialize(locale);
		_runningGame.Start();

		var map = new VisualRectangularHexMap3D(locale.Map, hexLayout);

		var sceneViewController = new SceneViewController(Camera.main!, map,
		                                                  CommandRouter);
		AddChildController(sceneViewController);
		await sceneViewController.Start();

		sceneViewController.PositionCameraToOverview();

		SetMode(_arrival_Mode);
	}


	protected override void DoUpdate()
	{
		_mode.Update();
	}


	public override void UpdateViewModel()
	{
		_uiVM.Update();
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void Compose(out ILocaleFactory localeFactory,
	                     out IRunningGameInitializer runningGameInitializer,
	                     out HexLayout3D hexLayout)
	{
		_runningGame = new RunningGameInstance(
			new World_Adapter(new Time_Adapter(), new Map_Adapter(), new Band_Adapter()));

		_scenePresentationModel = new ScenePresentationModel();

		var localeRepository = new LocaleAssetRepository(GameDatabase.Instance.Domain.Locales);
		localeFactory = new LocaleFactory(localeRepository);

		hexLayout = new HexLayout3D(
			new HexLayout(HexOrientation),
			new Matrix3x2(Vector3.right, Vector3.forward));

		var terrainTypePresentationRepository =
			new TerrainTypePresentationRepository(GameDatabase.Instance.Presentation.TerrainTypes, hexLayout);
		var resourceTypePresentationRepository =
			new ResourceTypePresentationRepository(GameDatabase.Instance.Presentation.ResourceTypes);
		var humanTypePresentationRepository = new HumanTypePresentationRepository();

		runningGameInitializer = Create_RunningGameInitializer(
			hexLayout, terrainTypePresentationRepository, resourceTypePresentationRepository);

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


	private IRunningGameInitializer Create_RunningGameInitializer(
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


	#region Command handlers

	private void OnEnterPlaceCampMode(EnterPlaceCampMode command)
	{
		SetMode(_campPlacing_Mode);
	}


	private void OnPlaceCamp(PlaceCamp command)
	{
		_runningGame.PlaceCamp(command.Position);
		SetMode(_interPeriod_Mode);
	}


	private void OnRunYearPeriod(RunYearPeriod command)
	{
		_runningGame.RunYearPeriod();
		SetMode(_periodRunning_Mode);
	}


	private void OnYearPeriodChanged(YearPeriodChanged evt)
	{
		SetMode(_interPeriod_Mode);
	}


	private void OnHoveredTileChanged(HoveredTileChanged evt)
	{
		_scenePresentationModel.HoveredTile = evt.Position;
	}

	#endregion


	private void SetMode(IMode mode)
	{
		if (_mode == mode)
			return;

		// _mode is null before initialization
		// ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
		_mode?.Exit();

		_mode = mode;
		_mode.Enter();
	}
}



}
