using Unity.Entities;
using Unity.Properties;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.GameTime.Components;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public class GameVM : IViewModel
{
	[CreateProperty]
	public string Month { get; private set; }

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
		Month = string.Empty;

		BandMembersVM = new BandMembersVM(bandMemberTypeRepository);

		TileInfoVM = new TileInfoVM(terrainTypeRepository, resourceTypeRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		EndTurnCommand = new EndTurn_CommandVM(
			() => commandRouter.EmitCommand(new EndTurnCommand(), controller));
	}


	public void Update()
	{
		UpdateYearPeriod();
		BandMembersVM.Update();
		TileInfoVM.Update();
	}


	private void UpdateYearPeriod()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<CurrentYearPeriod>());
		var yearPeriod = query.GetSingleton<CurrentYearPeriod>();

		Month = yearPeriod.Value.Month.ToString();
	}
}



}
