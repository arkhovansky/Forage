using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Rules;
using App.Game.ECS.BandMember.Energy.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.Gathering.Rules;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Gathering.Systems {



[UpdateInGroup(typeof(DomainSimulation))]
public partial struct Gathering_System : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var hoursDelta = SystemAPI.GetSingleton<GameTime.Components.GameTime>().DeltaHours;
		float innerCellDiameter = SystemAPI.GetSingleton<PhysicalMapParameters>().TileInnerDiameter;

		foreach (var (gatherer,
			         gatheringActivity,
			         gatheringActivityEnabled, activityEnabled,
			         foodConsumer)
		         in SystemAPI.Query<
			         Gatherer,
			         GatheringActivity,
			         EnabledRefRW<GatheringActivity>, EnabledRefRW<Activity>,
			         RefRW<FoodConsumer>
			         >())
		{
			var ripeBiomass = SystemAPI.GetComponentRW<RipeBiomass>(gatheringActivity.ResourceEntity);

			Gathering_Rules.Gather(ref ripeBiomass.ValueRW, ref foodConsumer.ValueRW,
			                       gatherer, hoursDelta, innerCellDiameter);

			if (!AI_Rules.Should_GatherOnTile(foodConsumer.ValueRO, ripeBiomass.ValueRO)) {
				activityEnabled.ValueRW = false;
				gatheringActivityEnabled.ValueRW = false;
			}
		}
	}
}



}
