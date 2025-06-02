using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.Util;
using App.Game.ECS.Util.Components;



namespace App.Game.ECS.GameTime {



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
			if (SystemAPI.HasSingleton<PlayYearPeriod>()) {
				state.EntityManager.AddComponent<GameTimeRun>(singletonEntity);

				state.EntityManager.RemoveComponent<PlayYearPeriod>(singletonEntity);
			}

			if (SystemAPI.HasSingleton<GameTimeRun>()) {
				var gameTime = SystemAPI.GetSingletonRW<Components.GameTime>();
				yearPeriodChanged =
					gameTime.ValueRW.AdvanceTillNextYearPeriod(SystemAPI.Time.DeltaTime * gameTimeScale);
			}
		}

		if (yearPeriodChanged) {
			state.EntityManager.AddComponent<StopGameTime>(singletonEntity);
			state.EntityManager.AddComponentData(singletonEntity, new YearPeriodChanged_Event());
		}
		else {
			state.EntityManager.RemoveComponent<YearPeriodChanged_Event>(singletonEntity);
		}
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }
}



}
