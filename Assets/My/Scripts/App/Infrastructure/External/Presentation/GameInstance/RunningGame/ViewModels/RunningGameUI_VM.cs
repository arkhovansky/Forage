using System;
using System.Collections.Generic;

using Unity.Properties;

using Lib.AppFlow;
using Lib.UICore.Mvvm;

using App.Application.Flow.GameInstance.RunningGame;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Application.Flow.GameInstance.RunningGame.Presentation;
using App.Game.Models;
using App.Infrastructure.Common.Contracts.Database.Presentation;
using App.Infrastructure.External.Presentation.GameInstance.RunningGame.ViewModels.Children;



namespace App.Infrastructure.External.Presentation.GameInstance.RunningGame.ViewModels {



public partial class RunningGameUI_VM : IRunningGameUI_VM, IViewModel
{
	[CreateProperty]
	public string GameTime { get; private set; }

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

	private readonly IRunningGame_PresentationModel_RO _presentationModel;

	private readonly Dictionary<ModeId, IMode> _modes = new();

	private IMode _mode = null!;

	//----------------------------------------------------------------------------------------------


	public RunningGameUI_VM(IRunningGameInstance_RO runningGameInstance,
	                        IRunningGame_PresentationModel_RO presentationModel,
	                        IController controller,
	                        ICommandRouter commandRouter,
	                        ITerrainTypePresentationRepository terrainTypePresentationRepository,
	                        IResourceTypePresentationRepository resourceTypePresentationRepository,
	                        IHumanTypePresentationRepository humanTypePresentationRepository)
	{
		_game = runningGameInstance;
		_presentationModel = presentationModel;

		GameTime = string.Empty;

		BandMembersVM = new BandMembersVM(_game.World.Band, _game.World.Time, humanTypePresentationRepository);

		TileInfoVM = new TileInfoVM(_game.World.Map, presentationModel,
		                            terrainTypePresentationRepository, resourceTypePresentationRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		RunYearPeriodCommand = new RunYearPeriod_CommandVM(
			() => commandRouter.EmitCommand(new RunYearPeriod(), controller));

		SelectCampLocationHintVM = new SelectCampLocationHintVM();

		// Should come at the end since modes might use data members of this
		_modes.Add(ModeId.Arrival, new Arrival_Mode(this));
		_modes.Add(ModeId.CampPlacing, new CampPlacing_Mode(this));
		_modes.Add(ModeId.InterPeriod, new InterPeriod_Mode(this));
		_modes.Add(ModeId.PeriodRunning, new PeriodRunning_Mode(this));
	}


	public void Update()
	{
		UpdateMode();

		UpdateGameTime();
		BandMembersVM.Update();
		TileInfoVM.Update();
	}

	//----------------------------------------------------------------------------------------------
	// private

	private void UpdateMode()
	{
		var gamePhase = _game.GamePhase;
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


	private void UpdateGameTime()
	{
		var gameTime = _game.World.Time.Get_Time();
		bool daylight = _game.World.Time.Get_IsDaylight();

		var partOfDay = daylight ? "Day" : "Night";
		GameTime = $"{gameTime.YearPeriod.Month.ToString()}   Day: {gameTime.Day}   Hour: {(uint)gameTime.Hours}   " +
		           $"({partOfDay})";
	}
}



}
