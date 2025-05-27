using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;
using UnityEngine.Assertions;

using App.Client.Framework.UICore.Mvvm;
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
