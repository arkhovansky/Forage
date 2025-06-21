using System;

using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember {



[UpdateInGroup(typeof(DomainSimulation))]
public partial struct Gathering : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var hoursDelta = SystemAPI.GetSingleton<GameTime.Components.GameTime>().DeltaHours;

		foreach (var (forager,
			         gatheringActivity,
			         gatheringActivityEnabled, activityEnabled,
			         foodConsumer)
		         in SystemAPI.Query<
			         Forager,
			         GatheringActivity,
			         EnabledRefRW<GatheringActivity>, EnabledRefRW<Activity>,
			         RefRW<FoodConsumer>
			         >())
		{
			var ripeBiomass = SystemAPI.GetComponentRW<RipeBiomass>(gatheringActivity.ResourceEntity);

			const float EnergyDensity_kcalPerKg = 1000;
			float massCanGather = forager.GatheringSpeed * hoursDelta;
			float neededMass = foodConsumer.ValueRO.EnergyStillNeeded / EnergyDensity_kcalPerKg;

			float wantedMass = Math.Min(massCanGather, neededMass);
			float gatheredMass = Math.Min(wantedMass, ripeBiomass.ValueRO.Value);

			if (gatheredMass > 0) {
				ripeBiomass.ValueRW.Decrease(gatheredMass);
				foodConsumer.ValueRW.ConsumeEnergy(gatheredMass * EnergyDensity_kcalPerKg);
			}


			// Possibly stop activity
			if (foodConsumer.ValueRO.IsSatiated || ripeBiomass.ValueRO.IsZero) {
				activityEnabled.ValueRW = false;
				gatheringActivityEnabled.ValueRW = false;
			}
		}
	}
}



}
