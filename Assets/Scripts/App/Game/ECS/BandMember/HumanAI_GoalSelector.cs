using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Components;
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
			var (position, entity)
			in SystemAPI.Query<
				RefRO<TilePosition>
				>()
				.WithAll<Human>()
				.WithNone<GoalComponent>()
				.WithEntityAccess())
		{
			Goal goal = Goal.Forage;

			SystemAPI.SetComponent(entity, new GoalComponent(goal));
			SystemAPI.SetComponentEnabled<GoalComponent>(entity, true);

			SystemAPI.SetComponent(entity, new Foraging());
			SystemAPI.SetComponentEnabled<Foraging>(entity, true);
		}
	}
}



}
