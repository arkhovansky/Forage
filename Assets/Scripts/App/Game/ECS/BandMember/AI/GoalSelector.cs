using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
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
		foreach (
			var (foodConsumer, entity)
			in SystemAPI.Query<
				FoodConsumer
				>()
				.WithAll<Human>()
				.WithDisabled<Task>()
				.WithEntityAccess())
		{
			if (!foodConsumer.IsSatiated) {
				SystemAPI.SetComponent(entity, new GoalComponent(Goal.Forage));
				SystemAPI.SetComponentEnabled<Forage_Goal>(entity, true);
			}
			else {  // Satiated
				// Remove current task
				SystemAPI.SetComponentEnabled<Forage_Goal>(entity, false);

				// Set new goal
				SystemAPI.SetComponent(entity, new GoalComponent(Goal.Leisure));
				SystemAPI.SetComponentEnabled<Leisure_Goal>(entity, true);
			}

			SystemAPI.SetComponentEnabled<GoalComponent>(entity, true);
		}
	}
}



}
