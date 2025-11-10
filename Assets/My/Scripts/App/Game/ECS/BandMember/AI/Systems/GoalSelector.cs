using System;

using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.AI.Rules;
using App.Game.ECS.BandMember.Energy.Components;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI.Systems {



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
			var goal = AI_Rules.SelectGoal(daylight, foodConsumer);

			switch (goal) {
				case Goal.Forage: SetForageGoal(entity, ref state); break;
				case Goal.Leisure: SetLeisureGoal(entity, ref state); break;
				case Goal.Sleep: SetSleepGoal(entity, ref state); break;
				default:
					throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
			}
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
