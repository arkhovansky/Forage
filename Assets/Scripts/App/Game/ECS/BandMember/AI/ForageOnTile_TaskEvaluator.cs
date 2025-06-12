using Unity.Assertions;
using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.AI {



[UpdateInGroup(typeof(HumanAI))]
[UpdateBefore(typeof(GoalSelector))]
public partial struct ForageOnTile_TaskEvaluator : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (foodConsumer,
			         forageTask, forageTaskEnabled, taskEnabled,
			         entity) in
		         SystemAPI.Query<
				         FoodConsumer,
				         RefRO<ForageOnTile_Task>, EnabledRefRW<ForageOnTile_Task>, EnabledRefRW<Task>
			         >()
			         .WithDisabled<Activity>()
			         .WithEntityAccess())
		{
			var resourceBiomass = SystemAPI.GetComponent<RipeBiomass>(forageTask.ValueRO.TargetResourceEntity);

			Assert.IsTrue(SystemAPI.GetComponent<MapPosition>(entity).Position == forageTask.ValueRO.Position);

			if (foodConsumer.IsSatiated || resourceBiomass.IsZero) {
				taskEnabled.ValueRW = false;
				forageTaskEnabled.ValueRW = false;
			}
			else {
				StartGatheringActivity(entity, forageTask.ValueRO.TargetResourceEntity, ref state);
			}
		}
	}


	private void StartGatheringActivity(Entity entity, Entity resourceEntity, ref SystemState state)
	{
		SystemAPI.SetComponentEnabled<Activity>(entity, true);

		SystemAPI.SetComponent(entity, new GatheringActivity(resourceEntity));
		SystemAPI.SetComponentEnabled<GatheringActivity>(entity, true);
	}
}



}
