using System;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using App.Game.Database;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.Prefabs.Components;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public class BandInitializer : IBandInitializer
{
	private readonly IHumanTypeRepository _humanTypeRepository;


	//----------------------------------------------------------------------------------------------


	public BandInitializer(IHumanTypeRepository humanTypeRepository)
	{
		_humanTypeRepository = humanTypeRepository;
	}



	public void Init(IDictionary<uint, uint> humanTypeCounts)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prefabReferences =
			entityManager.CreateEntityQuery(typeof(PrefabReferences)).GetSingleton<PrefabReferences>();

		int iBandMember = 0;
		foreach (var (typeId, countOfType) in humanTypeCounts) {
			var humanType = _humanTypeRepository.Get(typeId);

			var prefabEntity = humanType.Gender switch {
				Gender.Male => prefabReferences.Man,
				Gender.Female => prefabReferences.Woman,
				_ => throw new ArgumentOutOfRangeException()
			};

			var clonedEntities = new NativeArray<Entity>((int)countOfType, Allocator.Temp);
			entityManager.Instantiate(prefabEntity, clonedEntities);

			for (int i = 0; i < countOfType; i++) {
				var entity = clonedEntities[i];

				entityManager.SetComponentData(entity, new BandMember {Id = iBandMember++});
			}

			clonedEntities.Dispose();
		}
	}
}



}
