using Unity.Properties;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Application.PresentationDatabase;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class RunningGameUI_VM : IViewModel
{
	[CreateProperty]
	public string GameTime { get; private set; }

	public BandMembersVM BandMembersVM { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public RunYearPeriod_CommandVM RunYearPeriodCommand { get; }

	public SelectCampLocationHintVM SelectCampLocationHintVM { get; }



	private readonly IRunningGameInstance_RO _game;



	public RunningGameUI_VM(IRunningGameInstance_RO runningGameInstance,
	                        IScenePresentationModel_RO presentationModel,
	                        IController controller,
	                        ICommandRouter commandRouter,
	                        ITerrainTypePresentationRepository terrainTypePresentationRepository,
	                        IResourceTypePresentationRepository resourceTypePresentationRepository,
	                        IHumanTypePresentationRepository humanTypePresentationRepository)
	{
		_game = runningGameInstance;

		GameTime = string.Empty;

		BandMembersVM = new BandMembersVM(_game.World.Band, _game.World.Time, humanTypePresentationRepository);

		TileInfoVM = new TileInfoVM(_game.World.Map, presentationModel,
		                            terrainTypePresentationRepository, resourceTypePresentationRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		RunYearPeriodCommand = new RunYearPeriod_CommandVM(
			() => commandRouter.EmitCommand(new RunYearPeriod(), controller));

		SelectCampLocationHintVM = new SelectCampLocationHintVM();
	}


	public void Update()
	{
		UpdateGameTime();
		BandMembersVM.Update();
		TileInfoVM.Update();
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
