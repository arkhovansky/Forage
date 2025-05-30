using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember {



[UpdateInGroup(typeof(HumanAI))]
[UpdateBefore(typeof(GoalSelector))]
public partial struct ForageOnTile_TaskEvaluator : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (foodConsumer,
			         forageTask, forageTaskEnabled, taskEnabled) in
		         SystemAPI.Query<
			         FoodConsumer,
			         RefRO<ForageOnTile_Task>, EnabledRefRW<ForageOnTile_Task>, EnabledRefRW<Task>
			         >()
			         .WithDisabled<Activity>())
		{
			var resourceBiomass = SystemAPI.GetComponent<RipeBiomass>(forageTask.ValueRO.TargetResourceEntity);

			if (foodConsumer.IsSatiated || resourceBiomass.IsZero) {
				taskEnabled.ValueRW = false;
				forageTaskEnabled.ValueRW = false;
			}
		}
	}
}



}
