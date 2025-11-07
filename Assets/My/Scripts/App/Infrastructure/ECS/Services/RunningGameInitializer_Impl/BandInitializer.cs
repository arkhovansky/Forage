using System;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Prefabs.Components;
using App.Services.BandMembers;



namespace App.Infrastructure.ECS.Services.RunningGameInitializer_Impl {



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

		var prefabReferences =
			entityManager.CreateEntityQuery(typeof(PrefabReferences)).GetSingleton<PrefabReferences>();

		int iBandMember = 0;
		foreach (var (bandMemberTypeId, memberCountOfType) in bandMemberTypeCounts) {
			var bandMemberType = _bandMemberTypeRepository.Get(bandMemberTypeId);

			var prefabEntity = bandMemberType.Gender switch {
				Gender.Male => prefabReferences.Man,
				Gender.Female => prefabReferences.Woman,
				_ => throw new ArgumentOutOfRangeException()
			};

			var clonedEntities = new NativeArray<Entity>((int)memberCountOfType, Allocator.Temp);
			entityManager.Instantiate(prefabEntity, clonedEntities);

			for (int i = 0; i < memberCountOfType; i++) {
				var entity = clonedEntities[i];

				entityManager.SetComponentData(entity, new BandMember {Id = iBandMember++});
			}

			clonedEntities.Dispose();
		}
	}
}



}
