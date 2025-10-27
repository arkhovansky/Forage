using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



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
			// Possibly stop activity
			if (daylight) {
				activityEnabled.ValueRW = false;
				sleepingActivityEnabled.ValueRW = false;
			}
		}
	}
}



}
