using Unity.Burst;
using Unity.Entities;

using App.Game.Ecs.Components.Singletons;
using App.Game.Ecs.Components.Singletons.YearPeriod;



namespace App.Game.Ecs.Systems {



public partial struct GameTimeSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingleton<CurrentYearPeriod>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singleton = SystemAPI.GetSingletonEntity<CurrentYearPeriod>();

		if (SystemAPI.HasSingleton<Initialization_Tag>()) {
			state.EntityManager.AddComponentData(singleton, new YearPeriodChanged_Event());
			state.EntityManager.RemoveComponent<Initialization_Tag>(singleton);
		}
		else if (SystemAPI.HasSingleton<AdvanceYearPeriod_Command>()) {
			var yearPeriod = SystemAPI.GetSingletonRW<CurrentYearPeriod>();
			yearPeriod.ValueRW.Value.Advance();

			state.EntityManager.AddComponentData(singleton, new YearPeriodChanged_Event());

			state.EntityManager.RemoveComponent<AdvanceYearPeriod_Command>(singleton);
		}
		else {
			state.EntityManager.RemoveComponent<YearPeriodChanged_Event>(singleton);
		}
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }
}



}
