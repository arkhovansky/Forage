using Unity.Entities;

using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Events;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models_Impl.Domain {



public class Time_Adapter : ITime
{
	private EntityManager _entityManager
		= World.DefaultGameObjectInjectionWorld.EntityManager;

	private readonly Entity _singletonEntity
		= EcsService.GetSingletonEntity();



	public GameTime Get_Time()
	{
		return _entityManager.GetComponentData<GameTime>(_singletonEntity);
	}


	public bool Get_DayChanged()
	{
		return _entityManager.HasComponent<DayChanged>(_singletonEntity);
	}

	public bool Get_YearPeriodChanged()
	{
		return _entityManager.HasComponent<YearPeriodChanged>(_singletonEntity);
	}


	public bool Get_IsDaylight()
	{
		return _entityManager.HasComponent<Daylight>(_singletonEntity);
	}
}



}
