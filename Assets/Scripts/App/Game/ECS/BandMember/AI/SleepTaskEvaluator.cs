using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



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
		var campPosition = SystemAPI.GetComponent<TilePosition>(campEntity).Position;

		foreach (var (position,
			         entity)
		         in SystemAPI.Query<
			         TilePosition
			         >()
			         .WithAll<Sleep_Task>()
			         .WithDisabled<Activity>()
			         .WithEntityAccess())
		{
			Assert.IsTrue(position.Position == campPosition);

			if (!daylight)
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
