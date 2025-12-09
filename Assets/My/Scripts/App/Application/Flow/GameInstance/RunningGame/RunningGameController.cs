using Cysharp.Threading.Tasks;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Flow.Impl;
using App.Application.Framework.UICore.Gui;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Application.Flow.GameInstance.RunningGame.ViewModels;
using App.Application.Services;
using App.Game.Meta;



namespace App.Application.Flow.GameInstance.RunningGame {



public partial class RunningGameController : Controller
{
	private interface IMode
	{
		void Enter() {}
		void Exit() {}

		void Update() {}
	}


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
		_inGameMode = Create_InGameMode();

		await _inGameMode.Enter();
		// GameDatabase.Instance is available now

		Compose(out var localeFactory,
		        out var runningGameInitializer);

		var locale = localeFactory.Create(_game.LocaleId);

		runningGameInitializer.Initialize(locale);
		_runningGame.Start();

		var sceneViewController = Create_SceneViewController(locale.Map);
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
