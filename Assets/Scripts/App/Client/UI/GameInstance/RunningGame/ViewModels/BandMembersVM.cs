using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;
using UnityEngine.Assertions;

using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Services.BandMembers;



namespace App.Client.UI.GameInstance.RunningGame {



public class BandMembersVM : IViewModel
{
	public List<BandMemberVM> BandMembers { get; }


	private readonly IBandMemberTypeRepository _bandMemberTypeRepository;



	public BandMembersVM(IBandMemberTypeRepository bandMemberTypeRepository)
	{
		BandMembers = new List<BandMemberVM>();

		_bandMemberTypeRepository = bandMemberTypeRepository;
	}


	public void Update()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(
			ComponentType.ReadOnly<BandMember>(),
			ComponentType.ReadOnly<Human>());

		var entities = query.ToEntityArray(Allocator.Temp);
		var bandMembers = query.ToComponentDataArray<BandMember>(Allocator.Temp);
		var humans = query.ToComponentDataArray<Human>(Allocator.Temp);

		if (BandMembers.Count == 0) {
			for (var i = 0; i < bandMembers.Length; i++) {
				var bandMemberVM = new BandMemberVM() {Id = bandMembers[i].Id};
				SetBandMemberVM(bandMemberVM, entities[i], humans[i]);
				BandMembers.Add(bandMemberVM);
			}
		}
		else {
			Assert.IsTrue(BandMembers.Count == bandMembers.Length);

			for (var i = 0; i < bandMembers.Length; i++) {
				var bandMemberVM = BandMembers.Find(x => x.Id == bandMembers[i].Id);
				SetBandMemberVM(bandMemberVM, entities[i], humans[i]);
			}
		}
	}


	private void SetBandMemberVM(BandMemberVM bandMemberVM, Entity entity, Human human)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		bandMemberVM.Gender = _bandMemberTypeRepository.Get(human.TypeId).Gender.ToString();

		bandMemberVM.Goal = entityManager.IsComponentEnabled<GoalComponent>(entity)
			? entityManager.GetComponentData<GoalComponent>(entity).Goal.ToString()
			: string.Empty;

		bandMemberVM.Activity = entityManager.IsComponentEnabled<Forage_Goal>(entity)
			? "Foraging"
			: string.Empty;
	}
}



}
