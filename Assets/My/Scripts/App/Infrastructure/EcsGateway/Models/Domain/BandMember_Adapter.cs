using Unity.Entities;

using App.Game.Core.Query;
using App.Game.Database;
using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.BandMember.Statistics.Components;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class BandMember_Adapter : IBandMember_RO
{
	private readonly Entity _entity;

	private EntityManager _entityManager
		= World.DefaultGameObjectInjectionWorld.EntityManager;



	public BandMember_Adapter(Entity entity, int id, HumanTypeId typeId)
	{
		_entity = entity;
		Id = id;
		TypeId = typeId;
	}


	public int Id { get; }

	public HumanTypeId TypeId { get; }


	public Goal? Get_Goal()
	{
		return _entityManager.IsComponentEnabled<GoalComponent>(_entity)
			? _entityManager.GetComponentData<GoalComponent>(_entity).Goal
			: null;
	}


	public IBandMember_RO.ActivityType? Get_Activity() {
		if (_entityManager.IsComponentEnabled<LeisureActivity>(_entity))
			return IBandMember_RO.ActivityType.Leisure;
		if (_entityManager.IsComponentEnabled<SleepingActivity>(_entity))
			return IBandMember_RO.ActivityType.Sleeping;
		if (_entityManager.IsComponentEnabled<MovementActivity>(_entity))
			return IBandMember_RO.ActivityType.Moving;
		if (_entityManager.IsComponentEnabled<GatheringActivity>(_entity))
			return IBandMember_RO.ActivityType.Gathering;
		return null;
	}


	public YearPeriodStatistics Get_YearPeriodStatistics()
	{
		return _entityManager.GetComponentData<YearPeriodStatistics>(_entity);
	}
}



}
