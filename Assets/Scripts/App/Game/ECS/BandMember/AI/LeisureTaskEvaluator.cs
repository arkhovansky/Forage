using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



[UpdateInGroup(typeof(HumanAI))]
[UpdateBefore(typeof(GoalSelector))]
public partial struct LeisureTaskEvaluator : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Camp.Components.Camp>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var campEntity = SystemAPI.GetSingletonEntity<Camp.Components.Camp>();
		var campPosition = SystemAPI.GetComponent<TilePosition>(campEntity).Position;

		foreach (var (position,
			         entity)
		         in SystemAPI.Query<
			         TilePosition
			         >()
			         .WithAll<Leisure_Task>()
			         .WithDisabled<Activity>()
			         .WithEntityAccess())
		{
			Assert.IsTrue(position.Position == campPosition);

			StartLeisureActivity(entity, ref state);
		}
	}


	private void StartLeisureActivity(Entity entity, ref SystemState state)
	{
		SystemAPI.SetComponentEnabled<Activity>(entity, true);

		SystemAPI.SetComponentEnabled<LeisureActivity>(entity, true);
	}
}



}
