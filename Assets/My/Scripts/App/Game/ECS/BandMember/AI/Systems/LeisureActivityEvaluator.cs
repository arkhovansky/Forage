using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Rules;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI.Systems {



[UpdateInGroup(typeof(HumanAI))]
[UpdateBefore(typeof(LeisureTaskEvaluator))]
public partial struct LeisureActivityEvaluator : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var daylight = SystemAPI.HasSingleton<Daylight>();

		foreach (var (leisureActivityEnabled, activityEnabled)
		         in SystemAPI.Query<
			         EnabledRefRW<LeisureActivity>, EnabledRefRW<Activity>
			         >()
			         .WithAll<LeisureActivity>())
		{
			if (!AI_Rules.Should_Leisure(daylight)) {
				activityEnabled.ValueRW = false;
				leisureActivityEnabled.ValueRW = false;
			}
		}
	}
}



}
