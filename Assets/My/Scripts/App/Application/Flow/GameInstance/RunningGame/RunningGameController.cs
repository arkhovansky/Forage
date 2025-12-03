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
using App.Infrastructure.External.Database.Domain.Repositories;
using App.Infrastructure.External.Database.DomainSettings.Repositories;
using App.Infrastructure.External.Database.Presentation.Repositories;



namespace App.Application.Flow.GameInstance.RunningGame {



public partial class RunningGameController : Controller
{
	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;


	private readonly IGameInstance _game;

	private readonly IRunningGameInstance _runningGame;

	private readonly VisualRectangularHexMap3D _map;

	private readonly IScenePresentationModel _scenePresentationModel;

	private readonly IInGameMode _inGameMode;

	private readonly IRunningGameInitializer _runningGameInitializer;

	private readonly RunningGameUI_VM _uiVM;
	private readonly RunningGameUI_View _uiView;

	private readonly Arrival_Mode _arrival_Mode;
	private readonly CampPlacing_Mode _campPlacing_Mode;
	private readonly PeriodRunning_Mode _periodRunning_Mode;
	private readonly InterPeriod_Mode _interPeriod_Mode;

	private IMode _mode;

	//----------------------------------------------------------------------------------------------


	private interface IMode
	{
		void Enter() {}
		void Exit() {}

		void Update() {}
	}

	//----------------------------------------------------------------------------------------------


	public RunningGameController(IGameInstance game,
	                             IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_game = game;

		_runningGame = new RunningGameInstance(
			new World_Adapter(new Time_Adapter(), new Map_Adapter(), new Band_Adapter()));

		var hexLayout = new HexLayout3D(
			new HexLayout(HexOrientation),
			new Matrix3x2(Vector3.right, Vector3.forward));
		_map = new VisualRectangularHexMap3D(_game.Scene.Map, hexLayout);

		_scenePresentationModel = new ScenePresentationModel();

		_inGameMode = new InGameMode();

		var terrainTypePresentationRepository = new TerrainTypePresentationRepository(hexLayout);
		var resourceTypePresentationRepository = new ResourceTypePresentationRepository();
		var humanTypePresentationRepository = new HumanTypePresentationRepository();

		_runningGameInitializer = Create_RunningGameInitializer(
			hexLayout, terrainTypePresentationRepository, resourceTypePresentationRepository);

		_uiVM = new RunningGameUI_VM(_runningGame, _scenePresentationModel, this,
			commandRouter,
			terrainTypePresentationRepository, resourceTypePresentationRepository, humanTypePresentationRepository);
		_uiView = new RunningGameUI_View(_uiVM,
			gui, vvmBinder);
		gui.AddView(_uiView);

		base.AddCommandHandler<EnterPlaceCampMode>(OnEnterPlaceCampMode);
		base.AddCommandHandler<PlaceCamp>(OnPlaceCamp);
		base.AddCommandHandler<RunYearPeriod>(OnRunYearPeriod);
		base.AddCommandHandler<YearPeriodChanged>(OnYearPeriodChanged);
		base.AddCommandHandler<HoveredTileChanged>(OnHoveredTileChanged);

		// Should come at the end since modes might use fields (or properties) of this
		_arrival_Mode = new Arrival_Mode(this);
		_campPlacing_Mode = new CampPlacing_Mode(this);
		_periodRunning_Mode = new PeriodRunning_Mode(this);
		_interPeriod_Mode = new InterPeriod_Mode(this);

		_mode = _arrival_Mode;
	}


	public override async UniTask Start()
	{
		await _inGameMode.Enter();

		_runningGameInitializer.Initialize(_game.Scene);

		_runningGame.Start();

		var sceneViewController = new SceneViewController(Camera.main!, _map,
		                                                  CommandRouter);
		AddChildController(sceneViewController);
		await sceneViewController.Start();

		sceneViewController.PositionCameraToOverview();

		_mode.Enter();
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


	private IRunningGameInitializer Create_RunningGameInitializer(
		HexLayout3D hexLayout,
		ITerrainTypePresentationRepository terrainTypePresentationRepository,
		IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		var terrainInitializer = new TerrainInitializer(hexLayout, terrainTypePresentationRepository);

		var resourceTypeRepository = new ResourceTypeRepository();
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

		var systemParametersRepository = new SystemParametersRepository();
		var domainSettingsRepository = new DomainSettingsRepository();
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

		_mode.Exit();
		_mode = mode;
		_mode.Enter();
	}
}



}
