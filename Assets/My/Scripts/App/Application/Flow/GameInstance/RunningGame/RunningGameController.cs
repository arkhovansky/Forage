using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.AppFlow.Impl;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Flow.GameInstance.RunningGame.Models.Domain;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Application.Flow.GameInstance.RunningGame.Presentation;
using App.Application.Services;
using App.Game.Meta;
using App.Game.Models;



namespace App.Application.Flow.GameInstance.RunningGame {



public partial class RunningGameController : Controller
{
	private interface IMode
	{
		void Enter() {}
		void Exit() {}

		void Update() {}
	}

	private class Arrival_Mode : IMode {}
	private class InterPeriod_Mode : IMode {}


	private readonly IGameInstance _game;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;

	private readonly Dictionary<ModeId, IMode> _modes = new();


	#region Semantically readonly

	private IInGameMode _inGameMode = null!;

	private IRunningGameInstance _runningGame = null!;

	private IRunningGame_PresentationModel _presentationModel = null!;

	private IRunningGameUI_VM _uiVM = null!;
	private IView _uiView = null!;

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

		UpdateMode();
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
		_presentationModel.Is_CampPlacing_Mode = true;
		UpdateMode();
	}


	private void OnPlaceCamp(PlaceCamp command)
	{
		_runningGame.PlaceCamp(command.Position);
		UpdateMode();
	}


	private void OnRunYearPeriod(RunYearPeriod command)
	{
		_runningGame.RunYearPeriod();
		UpdateMode();
	}


	private void OnYearPeriodChanged(YearPeriodChanged evt)
	{
		UpdateMode();
	}


	private void OnHoveredTileChanged(HoveredTileChanged evt)
	{
		_presentationModel.HoveredTile = evt.Position;
	}

	#endregion


	private void UpdateMode()
	{
		var gamePhase = _runningGame.GamePhase;
		bool isCampPlacing = _presentationModel.Is_CampPlacing_Mode;

		var modeId = gamePhase switch {
			GamePhase.Arrival =>
				isCampPlacing
					? ModeId.CampPlacing
					: ModeId.Arrival,

			GamePhase.InterPeriod => ModeId.InterPeriod,
			GamePhase.PeriodRunning => ModeId.PeriodRunning,

			_ => throw new ArgumentOutOfRangeException(nameof(gamePhase), gamePhase, null)
		};

		SetMode(modeId);
	}


	private void SetMode(ModeId modeId)
	{
		SetMode(_modes[modeId]);
	}


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
