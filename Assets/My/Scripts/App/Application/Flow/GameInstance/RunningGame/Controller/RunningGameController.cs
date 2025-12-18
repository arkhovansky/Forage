using System.Collections.Generic;

using Lib.AppFlow;

using App.Application.Flow.GameInstance.RunningGame.Messages.Commands;
using App.Application.Flow.GameInstance.RunningGame.Messages.InputEvents;
using App.Application.Flow.GameInstance.RunningGame.Models.UI;
using App.Application.Flow.GameInstance.RunningGame.Models.UI.Impl;
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
	private class PeriodRunning_Mode : IMode {}


	private readonly IRunningGameInstance _runningGame;

	private readonly IRunningGame_UIModel _uiModel;

	private readonly Dictionary<UIModeId, IMode> _modes = new();


	private IMode _mode = null!;


	//----------------------------------------------------------------------------------------------


	public RunningGameController(IRunningGameInstance runningGame,
	                             IRunningGame_UIModel uiModel,
	                             ICommand_Emitter commandEmitter)
		: base(commandEmitter)
	{
		_runningGame = runningGame;
		_uiModel = uiModel;

		base.Add_Command_Handler<EnterPlaceCampMode>(OnEnterPlaceCampMode);
		base.Add_Command_Handler<PlaceCamp>(OnPlaceCamp);
		base.Add_Command_Handler<RunYearPeriod>(OnRunYearPeriod);
		base.Add_InputEvent_Handler<HoveredTileChanged>(OnHoveredTileChanged);

		// Should come at the end since modes might use data members of this
		_modes.Add(UIModeId.Arrival, new Arrival_Mode());
		_modes.Add(UIModeId.CampPlacing, new CampPlacing_Mode(this));
		_modes.Add(UIModeId.InterPeriod, new InterPeriod_Mode());
		_modes.Add(UIModeId.PeriodRunning, new PeriodRunning_Mode());
	}


	//----------------------------------------------------------------------------------------------
	// ILoopComponent implementation overrides


	public override void Start()
	{
		UpdateMode();
	}


	public override void Update()
	{
		UpdateMode();
		_mode.Update();
	}


	//----------------------------------------------------------------------------------------------
	// private


	#region Message handlers

	private void OnEnterPlaceCampMode(EnterPlaceCampMode command)
	{
		_uiModel.Is_CampPlacing_Mode = true;
		UpdateMode();
	}


	private void OnPlaceCamp(PlaceCamp command)
	{
		_runningGame.PlaceCamp(command.Position);
	}


	private void OnRunYearPeriod(RunYearPeriod command)
	{
		_runningGame.RunYearPeriod();
	}


	private void OnHoveredTileChanged(HoveredTileChanged evt)
	{
		_uiModel.HighlightedTile = evt.Position;
	}

	#endregion


	private void UpdateMode()
	{
		var modeId = UIMode_Utils.CalculateUIMode(_runningGame.GamePhase, _uiModel.Is_CampPlacing_Mode);
		SetMode(modeId);
	}


	private void SetMode(UIModeId modeId)
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
