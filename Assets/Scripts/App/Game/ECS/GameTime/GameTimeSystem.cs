using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.Components.Singletons;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.Util;
using App.Game.ECS.Util.Components;



namespace App.Game.ECS.GameTime {



[CreateAfter(typeof(InitializationSystem))]
public partial struct GameTimeSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<SingletonEntity_Tag>();
		state.EntityManager.AddComponent<CurrentYearPeriod>(singletonEntity);
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singleton = SystemAPI.GetSingletonEntity<SingletonEntity_Tag>();

		if (SystemAPI.HasSingleton<Initialization_Tag>()) {
			state.EntityManager.AddComponentData(singleton, new YearPeriodChanged_Event());
			state.EntityManager.RemoveComponent<Initialization_Tag>(singleton);
		}
		else if (SystemAPI.HasSingleton<AdvanceYearPeriod>()) {
			var yearPeriod = SystemAPI.GetSingletonRW<CurrentYearPeriod>();
			yearPeriod.ValueRW.Value.Advance();

			state.EntityManager.AddComponentData(singleton, new YearPeriodChanged_Event());

			state.EntityManager.RemoveComponent<AdvanceYearPeriod>(singleton);
		}
		else {
			state.EntityManager.RemoveComponent<YearPeriodChanged_Event>(singleton);
		}
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }
}



}
