using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Game.ECS.BandMember.General.Components;



namespace App.Infrastructure.EcsGateway.Models_Impl.Domain {



public class Band_Adapter : IBand_RO
{
	public IReadOnlyList<IBandMember_RO> Get_Members()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(
			ComponentType.ReadOnly<BandMember>(),
			ComponentType.ReadOnly<Human>());

		var entities = query.ToEntityArray(Allocator.Temp);
		var bandMembers = query.ToComponentDataArray<BandMember>(Allocator.Temp);
		var humans = query.ToComponentDataArray<Human>(Allocator.Temp);

		var list = new List<IBandMember_RO>(entities.Length);

		for (var i = 0; i < bandMembers.Length; i++)
			list.Add(new BandMember_Adapter(entities[i], bandMembers[i].Id, humans[i].TypeId));

		return list;
	}
}



}
