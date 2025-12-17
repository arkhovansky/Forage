using System;
using System.Collections.Generic;

using Lib.AppFlow;
using Lib.UICore.Gui;

using App.Application.Flow.GameInstance.RunningGame;
using App.Application.Flow.GameInstance.RunningGame.Messages.Commands;
using App.Application.Flow.GameInstance.RunningGame.Models.UI;
using App.Game.Core;
using App.Game.Core.Query;
using App.Infrastructure.Common.Contracts.Database.Presentation;
using App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels.Children;



namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels {



public partial class RunningGameUI_VM
	: IViewModel,
	  ILoopComponent
{
	public GameTimeVM GameTimeVM { get; }

	public BandMembersVM BandMembersVM { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public RunYearPeriod_CommandVM RunYearPeriodCommand { get; }

	public SelectCampLocationHintVM SelectCampLocationHintVM { get; }

	//----------------------------------------------------------------------------------------------


	private interface IMode
	{
		void Enter() {}
		void Exit() {}

		void Update() {}
	}


	private readonly IRunningGameInstance_RO _game;

	private readonly IRunningGame_UIModel_RO _uiModel;

	private readonly Dictionary<ModeId, IMode> _modes = new();

	private IMode _mode = null!;

	//----------------------------------------------------------------------------------------------


	public RunningGameUI_VM(IRunningGameInstance_RO runningGameInstance,
	                        IRunningGame_UIModel_RO uiModel,
	                        ICommand_Emitter commandEmitter,
	                        ITerrainTypePresentationRepository terrainTypePresentationRepository,
	                        IResourceTypePresentationRepository resourceTypePresentationRepository,
	                        IHumanTypePresentationRepository humanTypePresentationRepository)
	{
		_game = runningGameInstance;
		_uiModel = uiModel;

		GameTimeVM = new GameTimeVM(_game.World.Time);

		BandMembersVM = new BandMembersVM(_game.World.Band, _game.World.Time, humanTypePresentationRepository);

		TileInfoVM = new TileInfoVM(_game.World.Map, uiModel,
		                            terrainTypePresentationRepository, resourceTypePresentationRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandEmitter.Emit(new EnterPlaceCampMode()));
		RunYearPeriodCommand = new RunYearPeriod_CommandVM(
			() => commandEmitter.Emit(new RunYearPeriod()));

		SelectCampLocationHintVM = new SelectCampLocationHintVM();

		// Should come at the end since modes might use data members of this
		_modes.Add(ModeId.Arrival, new Arrival_Mode(this));
		_modes.Add(ModeId.CampPlacing, new CampPlacing_Mode(this));
		_modes.Add(ModeId.InterPeriod, new InterPeriod_Mode(this));
		_modes.Add(ModeId.PeriodRunning, new PeriodRunning_Mode(this));
	}


	//----------------------------------------------------------------------------------------------
	// ILoopComponent implementation


	public void LateUpdate()
	{
		UpdateMode();
		_mode.Update();

		UpdatePresentationData();
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void UpdateSimulationData()
	{
		GameTimeVM.Update();
		BandMembersVM.Update();
	}

	private void UpdatePresentationData()
	{
		TileInfoVM.Update();
	}


	private void UpdateMode()
	{
		var gamePhase = _game.GamePhase;
		bool isCampPlacing = _uiModel.Is_CampPlacing_Mode;

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


	public void SetMode(ModeId modeId)
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
