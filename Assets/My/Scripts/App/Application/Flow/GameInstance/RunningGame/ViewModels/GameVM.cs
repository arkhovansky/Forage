using Unity.Entities;
using Unity.Properties;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models;
using App.Game.Database;
using App.Game.ECS.GameTime.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class GameVM : IViewModel
{
	[CreateProperty]
	public string GameTime { get; private set; }

	public BandMembersVM BandMembersVM { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public RunYearPeriod_CommandVM RunYearPeriodCommand { get; }



	public GameVM(IScenePresentationModel presentationModel,
	              IController controller,
	              ICommandRouter commandRouter,
	              ITerrainTypeRepository terrainTypeRepository,
	              IResourceTypeRepository resourceTypeRepository,
	              IBandMemberTypeRepository bandMemberTypeRepository)
	{
		GameTime = string.Empty;

		BandMembersVM = new BandMembersVM(bandMemberTypeRepository);

		TileInfoVM = new TileInfoVM(presentationModel, terrainTypeRepository, resourceTypeRepository);

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
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = EcsService.GetSingletonEntity();

		var gameTime = entityManager.GetComponentData<GameTime>(singletonEntity);
		bool daylight = entityManager.HasComponent<Daylight>(singletonEntity);

		var partOfDay = daylight ? "Day" : "Night";
		GameTime = $"{gameTime.YearPeriod.Month.ToString()}   Day: {gameTime.Day}   Hour: {(uint)gameTime.Hours}   " +
		           $"({partOfDay})";
	}
}



}
