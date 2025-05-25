using System.Collections.Generic;

using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Properties;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.GameTime.Components;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public class GameVM : IViewModel
{
	[CreateProperty]
	public string Month { get; private set; }

	public List<BandMemberVM> BandMembers { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public EndTurn_CommandVM EndTurnCommand { get; }



	private readonly IBandMemberTypeRepository _bandMemberTypeRepository;



	public GameVM(IController controller,
	              ICommandRouter commandRouter,
	              ITerrainTypeRepository terrainTypeRepository,
	              IResourceTypeRepository resourceTypeRepository,
	              IBandMemberTypeRepository bandMemberTypeRepository)
	{
		Month = string.Empty;

		BandMembers = new List<BandMemberVM>();

		TileInfoVM = new TileInfoVM(terrainTypeRepository, resourceTypeRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		EndTurnCommand = new EndTurn_CommandVM(
			() => commandRouter.EmitCommand(new EndTurnCommand(), controller));

		_bandMemberTypeRepository = bandMemberTypeRepository;
	}


	public void Update()
	{
		UpdateYearPeriod();
		UpdateBandMembers();
		TileInfoVM.Update();
	}


	private void UpdateYearPeriod()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<CurrentYearPeriod>());
		var yearPeriod = query.GetSingleton<CurrentYearPeriod>();

		Month = yearPeriod.Value.Month.ToString();
	}


	private void UpdateBandMembers()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(
			ComponentType.ReadOnly<BandMember>(),
			ComponentType.ReadOnly<Human>(),
			ComponentType.ReadOnly<Forager>());

		var bandMembers = query.ToComponentDataArray<BandMember>(Allocator.Temp);
		var humans = query.ToComponentDataArray<Human>(Allocator.Temp);
		var foragers = query.ToComponentDataArray<Forager>(Allocator.Temp);

		if (BandMembers.Count == 0) {
			for (var i = 0; i < bandMembers.Length; i++) {
				BandMembers.Add(new BandMemberVM() {
					Id = bandMembers[i].Id,
					Gender = _bandMemberTypeRepository.Get(humans[i].TypeId).Gender.ToString(),
					Assignment = foragers[i].Activity.ToString()
				});
			}
		}
		else {
			Assert.IsTrue(BandMembers.Count == bandMembers.Length);

			for (var i = 0; i < bandMembers.Length; i++) {
				var bandMemberVM = BandMembers.Find(x => x.Id == bandMembers[i].Id);

				bandMemberVM.Gender = _bandMemberTypeRepository.Get(humans[i].TypeId).Gender.ToString();
				bandMemberVM.Assignment = foragers[i].Activity.ToString();
			}
		}
	}
}



}
