using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Rules;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI.Systems {



[UpdateInGroup(typeof(HumanAI))]
[UpdateBefore(typeof(SleepTaskEvaluator))]
public partial struct SleepingActivityEvaluator : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var daylight = SystemAPI.HasSingleton<Daylight>();

		foreach (var (sleepingActivityEnabled, activityEnabled)
		         in SystemAPI.Query<
			         EnabledRefRW<SleepingActivity>, EnabledRefRW<Activity>
			         >()
			         .WithAll<SleepingActivity>())
		{
			if (!AI_Rules.Should_Sleep(daylight)) {
				activityEnabled.ValueRW = false;
				sleepingActivityEnabled.ValueRW = false;
			}
		}
	}
}



}
