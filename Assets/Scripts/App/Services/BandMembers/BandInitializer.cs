using System.Collections.Generic;

using App.Game.Ecs.Components.BandMember;

using Unity.Collections;
using Unity.Entities;

using Lib.Util;



namespace App.Services.BandMembers {



public class BandInitializer : IBandInitializer
{
	private readonly IBandMemberTypeRepository _bandMemberTypeRepository;


	//----------------------------------------------------------------------------------------------


	public BandInitializer(IBandMemberTypeRepository bandMemberTypeRepository)
	{
		_bandMemberTypeRepository = bandMemberTypeRepository;
	}



	public void Init(IDictionary<uint, uint> bandMemberTypeCounts)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prototype = entityManager.CreateEntity();
		entityManager.AddComponent<BandMember>(prototype);
		entityManager.AddComponent<Human>(prototype);
		entityManager.AddComponent<Forager>(prototype);

		var count = bandMemberTypeCounts.Values.Sum();

		var clonedEntities = new NativeArray<Entity>((int)count, Allocator.Temp);
		entityManager.Instantiate(prototype, clonedEntities);


		int iBandMember = 0;
		foreach (var bandMemberTypeKV in bandMemberTypeCounts) {
			var bandMemberTypeId = bandMemberTypeKV.Key;
			var bandMemberType = _bandMemberTypeRepository.Get(bandMemberTypeId);

			var memberCountOfType = bandMemberTypeKV.Value;
			for (int i = 0; i < memberCountOfType; i++) {
				var entity = clonedEntities[iBandMember++];

				entityManager.SetComponentData(entity, new BandMember {Id = iBandMember});

				entityManager.SetComponentData(entity, new Human {TypeId = bandMemberTypeId});

				entityManager.SetComponentData(entity, new Forager {
					GatheringSpeed = bandMemberType.GatheringSpeed,
					Activity = Activity.Idle
				});
			}
		}

		clonedEntities.Dispose();

		entityManager.DestroyEntity(prototype);
	}
}



}
