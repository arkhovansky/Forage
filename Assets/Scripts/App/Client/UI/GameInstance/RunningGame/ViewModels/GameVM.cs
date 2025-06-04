using Unity.Entities;
using Unity.Properties;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.GameTime.Components;
using App.Services;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public class GameVM : IViewModel
{
	[CreateProperty]
	public string GameTime { get; private set; }

	public BandMembersVM BandMembersVM { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public EndTurn_CommandVM EndTurnCommand { get; }



	public GameVM(IController controller,
	              ICommandRouter commandRouter,
	              ITerrainTypeRepository terrainTypeRepository,
	              IResourceTypeRepository resourceTypeRepository,
	              IBandMemberTypeRepository bandMemberTypeRepository)
	{
		GameTime = string.Empty;

		BandMembersVM = new BandMembersVM(bandMemberTypeRepository);

		TileInfoVM = new TileInfoVM(terrainTypeRepository, resourceTypeRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		EndTurnCommand = new EndTurn_CommandVM(
			() => commandRouter.EmitCommand(new EndTurnCommand(), controller));
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
