using System;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Entities;

using Lib.Grid;

using App.Game.Core;
using App.Game.Core.Query;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.Camp.Components;
using App.Game.ECS.Camp.Components.Commands;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Game.Core {



public class Band_Adapter : IBand
{
	private readonly IEcsHelper _ecsHelper;

	//----------------------------------------------------------------------------------------------


	public Band_Adapter(IEcsHelper ecsHelper)
	{
		_ecsHelper = ecsHelper;
	}


	//----------------------------------------------------------------------------------------------
	// IBand_RO implementation


	public IReadOnlyList<IBandMember_RO> Get_Members()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		using var query = entityManager.CreateEntityQuery(
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


	//----------------------------------------------------------------------------------------------
	// IBand implementation


	public void PlaceCamp(AxialPosition position)
	{
		if (_ecsHelper.HasSingleton_Anywhere<Camp>())
			throw new InvalidOperationException("Camp already exists");

		_ecsHelper.SendEcsCommand(new PlaceCamp(position));
	}
}



}
