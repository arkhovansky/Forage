using Unity.Properties;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Game.Database;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class GameVM : IViewModel
{
	[CreateProperty]
	public string GameTime { get; private set; }

	public BandMembersVM BandMembersVM { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public RunYearPeriod_CommandVM RunYearPeriodCommand { get; }



	private readonly IRunningGameInstance_RO _game;



	public GameVM(IRunningGameInstance_RO runningGameInstance,
	              IScenePresentationModel_RO presentationModel,
	              IController controller,
	              ICommandRouter commandRouter,
	              ITerrainTypeRepository terrainTypeRepository,
	              IResourceTypeRepository resourceTypeRepository,
	              IBandMemberTypeRepository bandMemberTypeRepository)
	{
		_game = runningGameInstance;

		GameTime = string.Empty;

		BandMembersVM = new BandMembersVM(_game.World.Band, _game.World.Time, bandMemberTypeRepository);

		TileInfoVM = new TileInfoVM(_game.World.Map, presentationModel, terrainTypeRepository, resourceTypeRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		RunYearPeriodCommand = new RunYearPeriod_CommandVM(
			() => commandRouter.EmitCommand(new RunYearPeriod(), controller));
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
