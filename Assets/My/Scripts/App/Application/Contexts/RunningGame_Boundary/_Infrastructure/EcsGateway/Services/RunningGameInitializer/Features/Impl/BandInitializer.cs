using System;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.Database;
using App.Game.ECS.BandMember.AI.Initialization;
using App.Game.ECS.BandMember.Energy.Initialization;
using App.Game.ECS.BandMember.Gathering.Initialization;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.BandMember.General.Initialization;
using App.Game.ECS.BandMember.Movement.Initialization;
using App.Game.ECS.BandMember.Statistics.Initialization;
using App.Game.ECS.Prefabs.Components;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



public class BandInitializer : IBandInitializer
{
	private readonly IHumanTypeRepository _humanTypeRepository;


	//----------------------------------------------------------------------------------------------


	public BandInitializer(IHumanTypeRepository humanTypeRepository)
	{
		_humanTypeRepository = humanTypeRepository;
	}



	public void Init(IDictionary<HumanTypeId, uint> humanTypeCounts)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prefabReferences =
			entityManager.CreateEntityQuery(typeof(PrefabReferences)).GetSingleton<PrefabReferences>();

		int iBandMember = 0;
		foreach (var (typeId, countOfType) in humanTypeCounts) {
			var humanType = _humanTypeRepository.Get(typeId);

			var prefabEntity = humanType.Id switch {
				HumanTypeId.Man => prefabReferences.Man,
				HumanTypeId.Woman => prefabReferences.Woman,
				_ => throw new ArgumentOutOfRangeException()
			};

			InitializePrefab(prefabEntity, humanType);

			using var clonedEntities = new NativeArray<Entity>((int)countOfType, Allocator.Temp);
			entityManager.Instantiate(prefabEntity, clonedEntities);

			for (int i = 0; i < countOfType; i++) {
				var entity = clonedEntities[i];

				entityManager.SetComponentData(entity, new BandMember {Id = iBandMember++});
			}
		}
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void InitializePrefab(Entity prefabEntity, HumanType humanType)
	{
		General_HumanInitializer.Initialize(prefabEntity, humanType.Id);
		Energy_HumanInitializer.Initialize(prefabEntity, humanType.EnergyRequiredDaily);
		Movement_HumanInitializer.Initialize(prefabEntity, humanType.BaseSpeed);
		Gathering_HumanInitializer.Initialize(prefabEntity, humanType.GatheringSpeed);
		AI_HumanInitializer.Initialize(prefabEntity);
		Statistics_HumanInitializer.Initialize(prefabEntity);
	}
}



}
