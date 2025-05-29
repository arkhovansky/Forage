using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember {



[UpdateInGroup(typeof(HumanAI), OrderFirst = true)]
public partial struct HumanAI_GoalSelector : ISystem
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
				.WithNone<Activity>()
				.WithEntityAccess())
		{
			if (!foodConsumer.IsSatiated) {
				Goal goal = Goal.Forage;

				SystemAPI.SetComponent(entity, new GoalComponent(goal));
				SystemAPI.SetComponentEnabled<GoalComponent>(entity, true);

				SystemAPI.SetComponent(entity, new Foraging());
				SystemAPI.SetComponentEnabled<Foraging>(entity, true);
			}
			else {  // Satiated
				SystemAPI.SetComponentEnabled<Foraging>(entity, false);

				SystemAPI.SetComponent(entity, new GoalComponent(Goal.Leisure));
				SystemAPI.SetComponentEnabled<GoalComponent>(entity, true);
			}
		}
	}
}



}
