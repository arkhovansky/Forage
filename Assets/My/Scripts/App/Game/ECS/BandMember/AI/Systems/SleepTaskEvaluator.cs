using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.AI.Rules;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI.Systems {



[UpdateInGroup(typeof(HumanAI))]
[UpdateBefore(typeof(GoalSelector))]
public partial struct SleepTaskEvaluator : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Camp.Components.Camp>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var daylight = SystemAPI.HasSingleton<Daylight>();

		var campEntity = SystemAPI.GetSingletonEntity<Camp.Components.Camp>();
		var campPosition = SystemAPI.GetComponent<MapPosition>(campEntity).Value;

		foreach (var (position,
			         entity)
		         in SystemAPI.Query<
			         MapPosition
			         >()
			         .WithAll<Sleep_Task>()
			         .WithDisabled<Activity>()
			         .WithEntityAccess())
		{
			Assert.IsTrue(position == campPosition);

			if (AI_Rules.Should_Sleep(daylight))
				StartSleepActivity(entity, ref state);
			else
				StopTask(entity, ref state);
		}
	}


	private void StartSleepActivity(Entity entity, ref SystemState state)
	{
		SystemAPI.SetComponentEnabled<Activity>(entity, true);
		SystemAPI.SetComponentEnabled<SleepingActivity>(entity, true);
	}


	private void StopTask(Entity entity, ref SystemState state)
	{
		SystemAPI.SetComponentEnabled<Task>(entity, false);
		SystemAPI.SetComponentEnabled<Leisure_Task>(entity, false);
	}
}



}
