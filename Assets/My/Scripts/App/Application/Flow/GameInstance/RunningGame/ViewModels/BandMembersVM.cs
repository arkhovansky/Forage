using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;
using UnityEngine.Assertions;

using App.Application.Framework.UICore.Mvvm;
using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.BandMember.Statistics.Components;
using App.Game.ECS.GameTime.Components.Events;
using App.Services;
using App.Services.BandMembers;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



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

		if (entityManager.IsComponentEnabled<MovementActivity>(entity))
			bandMemberVM.Activity = "Moving";
		else if (entityManager.IsComponentEnabled<GatheringActivity>(entity))
			bandMemberVM.Activity = "Gathering";
		else if (entityManager.IsComponentEnabled<LeisureActivity>(entity))
			bandMemberVM.Activity = "Leisure";
		else if (entityManager.IsComponentEnabled<SleepingActivity>(entity))
			bandMemberVM.Activity = "Sleeping";
		else
			bandMemberVM.Activity = string.Empty;

		if (entityManager.HasComponent<DayChanged>(EcsService.GetSingletonEntity())) {
			var statistics = entityManager.GetComponentData<YearPeriodStatistics>(entity);

			const string format = "F1";

			bandMemberVM.AverageForagingHours = statistics.AverageForagingHours.ToString(format);
			bandMemberVM.AverageGatheringHours = statistics.AverageGatheringHours.ToString(format);
			bandMemberVM.AverageMovingHours = statistics.AverageMovingHours.ToString(format);
			bandMemberVM.AverageLeisureHours = statistics.AverageLeisureHours.ToString(format);
			bandMemberVM.AverageSleepingHours = statistics.AverageSleepingHours.ToString(format);
		}
	}
}



}
