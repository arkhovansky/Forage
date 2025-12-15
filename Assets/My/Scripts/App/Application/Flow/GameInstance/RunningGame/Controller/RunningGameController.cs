using System;
using System.Collections.Generic;

using Lib.AppFlow;

using App.Application.Flow.GameInstance.RunningGame.Messages.Commands;
using App.Application.Flow.GameInstance.RunningGame.Messages.InputEvents;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Game.Core;



namespace App.Application.Flow.GameInstance.RunningGame.Controller {



public partial class RunningGameController : Lib.AppFlow.Controller
{
	private interface IMode
	{
		void Enter() {}
		void Exit() {}

		void Update() {}
	}

	private class Arrival_Mode : IMode {}
	private class InterPeriod_Mode : IMode {}


	private readonly IRunningGameInstance _runningGame;

	private readonly IRunningGame_PresentationModel _presentationModel;

	private readonly ISceneViewController _sceneViewController;

	private readonly Dictionary<ModeId, IMode> _modes = new();


	private IMode _mode = null!;


	//----------------------------------------------------------------------------------------------


	public RunningGameController(IRunningGameInstance runningGame,
	                             IRunningGame_PresentationModel presentationModel,
	                             ISceneViewController sceneViewController,
	                             ICommand_Emitter commandEmitter)
		: base(commandEmitter)
	{
		_runningGame = runningGame;
		_presentationModel = presentationModel;
		_sceneViewController = sceneViewController;

		base.Add_Command_Handler<EnterPlaceCampMode>(OnEnterPlaceCampMode);
		base.Add_Command_Handler<PlaceCamp>(OnPlaceCamp);
		base.Add_Command_Handler<RunYearPeriod>(OnRunYearPeriod);
		base.Add_Command_Handler<YearPeriodChanged>(OnYearPeriodChanged);
		base.Add_InputEvent_Handler<HoveredTileChanged>(OnHoveredTileChanged);

		// Should come at the end since modes might use data members of this
		_modes.Add(ModeId.Arrival, new Arrival_Mode());
		_modes.Add(ModeId.CampPlacing, new CampPlacing_Mode(this));
		_modes.Add(ModeId.InterPeriod, new InterPeriod_Mode());
		_modes.Add(ModeId.PeriodRunning, new PeriodRunning_Mode(this));
	}


	public override void Start()
	{
		UpdateMode();
		_sceneViewController.PositionCameraToOverview();
	}


	public override void Update()
	{
		_mode.Update();
		_sceneViewController.Update();
	}


	//----------------------------------------------------------------------------------------------
	// private


	#region Message handlers

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
