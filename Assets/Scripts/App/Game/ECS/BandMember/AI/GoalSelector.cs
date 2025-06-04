using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



[UpdateInGroup(typeof(HumanAI))]
public partial struct GoalSelector : ISystem
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

		foreach (
			var (foodConsumer, entity)
			in SystemAPI.Query<
				FoodConsumer
				>()
				.WithAll<Human>()
				.WithDisabled<Task>()
				.WithEntityAccess())
		{
			if (daylight) {
				if (foodConsumer.IsSatiated)
					SetLeisureGoal(entity, ref state);
				else
					SetForageGoal(entity, ref state);
			}
			else
				SetSleepGoal(entity, ref state);
		}
	}


	private void SetForageGoal(Entity entity, ref SystemState state)
	{
		DisableGoalTags(entity, ref state);

		SystemAPI.SetComponent(entity, new GoalComponent(Goal.Forage));
		SystemAPI.SetComponentEnabled<GoalComponent>(entity, true);

		SystemAPI.SetComponentEnabled<Forage_Goal>(entity, true);
	}

	private void SetLeisureGoal(Entity entity, ref SystemState state)
	{
		DisableGoalTags(entity, ref state);

		SystemAPI.SetComponent(entity, new GoalComponent(Goal.Leisure));
		SystemAPI.SetComponentEnabled<GoalComponent>(entity, true);

		SystemAPI.SetComponentEnabled<Leisure_Goal>(entity, true);
	}

	private void SetSleepGoal(Entity entity, ref SystemState state)
	{
		DisableGoalTags(entity, ref state);

		SystemAPI.SetComponent(entity, new GoalComponent(Goal.Sleep));
		SystemAPI.SetComponentEnabled<GoalComponent>(entity, true);

		SystemAPI.SetComponentEnabled<Sleep_Goal>(entity, true);
	}


	private void DisableGoalTags(Entity entity, ref SystemState state)
	{
		SystemAPI.SetComponentEnabled<Forage_Goal>(entity, false);
		SystemAPI.SetComponentEnabled<Leisure_Goal>(entity, false);
		SystemAPI.SetComponentEnabled<Sleep_Goal>(entity, false);
	}
}



}
