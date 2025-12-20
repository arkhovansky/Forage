using Unity.Entities;

using App.Game.Core.Query;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Events;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class Time_Adapter : ITime
{
	private EntityManager _entityManager
		= World.DefaultGameObjectInjectionWorld.EntityManager;

	private readonly Entity _singletonEntity;

	//----------------------------------------------------------------------------------------------


	public Time_Adapter(IEcsHelper ecsHelper)
	{
		_singletonEntity = ecsHelper.GetSingletonEntity();
	}


	//----------------------------------------------------------------------------------------------
	// ITime implementation


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
