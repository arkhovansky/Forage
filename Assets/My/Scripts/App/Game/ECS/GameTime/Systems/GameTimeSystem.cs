using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.GameTime.Components.Events;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.Util.Components;
using App.Game.ECS.Util.Systems;



namespace App.Game.ECS.GameTime.Systems {



[CreateAfter(typeof(InitializationSystem))]
[UpdateInGroup(typeof(DomainSimulation), OrderFirst = true)]
public partial struct GameTimeSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<SingletonEntity_Tag>();
		state.EntityManager.AddComponent<Components.GameTime>(singletonEntity);
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<SingletonEntity_Tag>();

		const float gameTimeScale = 2f;

		bool dayChanged = false;
		bool yearPeriodChanged = false;

		if (SystemAPI.TryGetSingleton(out InitYearPeriod initYearPeriod)) {
			SystemAPI.SetSingleton(new Components.GameTime(initYearPeriod.YearPeriod, 1, 0));

			yearPeriodChanged = true;

			state.EntityManager.RemoveComponent<InitYearPeriod>(singletonEntity);
		}
		else if (SystemAPI.HasSingleton<StopGameTime>()) {
			state.EntityManager.RemoveComponent<GameTimeRun>(singletonEntity);

			var gameTime = SystemAPI.GetSingletonRW<Components.GameTime>();
			gameTime.ValueRW.DeltaHours = 0;

			state.EntityManager.RemoveComponent<StopGameTime>(singletonEntity);
		}
		else {
			if (SystemAPI.HasSingleton<RunYearPeriod>()) {
				state.EntityManager.AddComponent<GameTimeRun>(singletonEntity);

				state.EntityManager.RemoveComponent<RunYearPeriod>(singletonEntity);
			}

			if (SystemAPI.HasSingleton<GameTimeRun>()) {
				var gameTime = SystemAPI.GetSingletonRW<Components.GameTime>();
				gameTime.ValueRW.Advance(SystemAPI.Time.DeltaTime * gameTimeScale);

				dayChanged = gameTime.ValueRO.DayChanged;
				yearPeriodChanged = gameTime.ValueRO.YearPeriodChanged;
			}
		}

		if (dayChanged)
			state.EntityManager.AddComponent<DayChanged>(singletonEntity);
		else
			state.EntityManager.RemoveComponent<DayChanged>(singletonEntity);

		if (yearPeriodChanged) {
			state.EntityManager.AddComponent<StopGameTime>(singletonEntity);
			state.EntityManager.AddComponent<YearPeriodChanged>(singletonEntity);
		}
		else {
			state.EntityManager.RemoveComponent<YearPeriodChanged>(singletonEntity);
		}
	}
}



}
