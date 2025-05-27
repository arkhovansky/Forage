using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.Util;
using App.Game.ECS.Util.Components;



namespace App.Game.ECS.GameTime {



[CreateAfter(typeof(InitializationSystem))]
[UpdateInGroup(typeof(DiscreteActions))]
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

		bool yearPeriodChanged = false;

		if (SystemAPI.TryGetSingleton(out InitYearPeriod initYearPeriod)) {
			state.EntityManager.SetComponentData(singleton, new CurrentYearPeriod(initYearPeriod.YearPeriod));

			yearPeriodChanged = true;

			state.EntityManager.RemoveComponent<InitYearPeriod>(singleton);
		}
		else if (SystemAPI.HasSingleton<AdvanceYearPeriod>()) {
			var yearPeriod = SystemAPI.GetSingletonRW<CurrentYearPeriod>();
			yearPeriod.ValueRW.Value.Advance();

			yearPeriodChanged = true;

			state.EntityManager.RemoveComponent<AdvanceYearPeriod>(singleton);
		}

		if (yearPeriodChanged) {
			state.EntityManager.AddComponentData(singleton, new YearPeriodChanged_Event());
		}
		else {
			state.EntityManager.RemoveComponent<YearPeriodChanged_Event>(singleton);
		}
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }
}



}
