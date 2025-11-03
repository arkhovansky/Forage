using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;

using Cysharp.Threading.Tasks;

using Lib.VisualGrid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.HighLevel.Impl;
using App.Client.Framework.UICore.LowLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Client.UI.GameInstance.RunningGame.ViewModels;
using App.Game.ECS.Prefabs.Components;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Game.Meta;
using App.Services;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public partial class RunningGameController : Controller
{
	private const string GameSceneName = "Game";


	private readonly IGameInstance _game;

	private readonly VisualRectangularHexMap3D _map;

	private readonly GameVM _viewModel;
	private readonly GameView _uiView;

	private readonly IGameService _gameService;

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
	                             HexLayout3D hexLayout,
	                             ITerrainTypeRepository terrainTypeRepository,
	                             IResourceTypeRepository resourceTypeRepository,
	                             IBandMemberTypeRepository bandMemberTypeRepository,
	                             IGameService gameService,
	                             IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_game = game;

		_gameService = gameService;

		_map = new VisualRectangularHexMap3D(_game.Scene.Map, hexLayout);

		_viewModel = new GameVM(this,
			commandRouter,
			terrainTypeRepository, resourceTypeRepository, bandMemberTypeRepository);
		_uiView = new GameView(_viewModel,
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
		EcsService.SetEcsSystemsEnabled(true);
		GameSystems.Enabled = false;

		await LoadGameScene_Async();

		_gameService.PopulateWorld(_game.Scene);

		GameSystems.Enabled = true;

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
		_viewModel.Update();
	}


	#region Command handlers

	private void OnEnterPlaceCampMode(EnterPlaceCampMode command)
	{
		SetMode(_campPlacing_Mode);
	}


	private void OnPlaceCamp(PlaceCamp command)
	{
		EcsService.SendEcsCommand(new App.Game.ECS.Camp.Components.Commands.PlaceCamp(command.Position));

		SetMode(_periodRunning_Mode);
	}


	private void OnRunYearPeriod(RunYearPeriod command)
	{
		EcsService.SendEcsCommand(new Game.ECS.GameTime.Components.Commands.RunYearPeriod());

		SetMode(_periodRunning_Mode);
	}


	private void OnYearPeriodChanged(YearPeriodChanged evt)
	{
		SetMode(_interPeriod_Mode);
	}


	private void OnHoveredTileChanged(HoveredTileChanged evt)
	{
		EcsService.SendEcsCommand(new HoveredTileChanged_Event(evt.Position));
	}

	#endregion


	private async UniTask LoadGameScene_Async()
	{
		// Might be already loaded in editor
		if (SceneManager.GetSceneByName(GameSceneName).IsValid())
			return;

		await SceneManager.LoadSceneAsync(GameSceneName, LoadSceneMode.Additive);
		await WaitForSubsceneLoading();
	}


	private async UniTask WaitForSubsceneLoading()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<PrefabReferences>());

		await UniTask.WaitWhile(() => query.IsEmpty);
	}


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
