using System.Collections.Generic;

using Lib.AppFlow;
using Lib.UICore.Gui;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ViewModels.Children;
using App.Application.Contexts.RunningGame_Gameplay.Messages.Commands;
using App.Application.Contexts.RunningGame_Gameplay.Models.UI;
using App.Application.Contexts.RunningGame_Gameplay.Models.UI.Impl;
using App.Game.Core.Query;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ViewModels {



public partial class RunningGame_ScreenUI_VM
	: IViewModel,
	  ILoopComponent
{
	public GameTimeVM GameTimeVM { get; }

	public BandMembersVM BandMembersVM { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; private set; } = null!;
	public RunYearPeriod_CommandVM RunYearPeriodCommand { get; private set; } = null!;

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

	private readonly Dictionary<UIModeId, IMode> _modes = new();

	private IMode _mode = null!;

	//----------------------------------------------------------------------------------------------


	public RunningGame_ScreenUI_VM(IRunningGameInstance_RO runningGameInstance,
	                               IRunningGame_UIModel_RO uiModel,
	                               ITerrainType_TextualPresentation_Repository terrainTypePresentationRepository,
	                               IResourceType_TextualPresentation_Repository resourceTypePresentationRepository,
	                               IHumanType_TextualPresentation_Repository humanTypePresentationRepository)
	{
		_game = runningGameInstance;
		_uiModel = uiModel;

		GameTimeVM = new GameTimeVM(_game.World.Time);

		BandMembersVM = new BandMembersVM(_game.World.Band, _game.World.Time, humanTypePresentationRepository);

		TileInfoVM = new TileInfoVM(_game.World.Map, uiModel,
		                            terrainTypePresentationRepository, resourceTypePresentationRepository);

		SelectCampLocationHintVM = new SelectCampLocationHintVM();
	}


	public void Init_Command_Emitter(ICommand_Emitter commandEmitter)
	{
		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandEmitter.Emit(new EnterPlaceCampMode()));
		RunYearPeriodCommand = new RunYearPeriod_CommandVM(
			() => commandEmitter.Emit(new RunYearPeriod()));

		FinishInitialization();
	}


	private void FinishInitialization() {
		// Should come at the end of initialization, since modes might use data members of this
		_modes.Add(UIModeId.Arrival, new Arrival_Mode(this));
		_modes.Add(UIModeId.CampPlacing, new CampPlacing_Mode(this));
		_modes.Add(UIModeId.InterPeriod, new InterPeriod_Mode(this));
		_modes.Add(UIModeId.PeriodRunning, new PeriodRunning_Mode(this));
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
		var modeId = UIMode_Utils.CalculateUIMode(_game.GamePhase, _uiModel.Is_CampPlacing_Mode);
		SetMode(modeId);
	}


	public void SetMode(UIModeId modeId)
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
