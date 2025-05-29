using System;

using Unity.Burst;
using Unity.Entities;
using UnityEngine;

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
		const float gameTimeScale = 0.1f;
		var hoursDelta = SystemAPI.Time.DeltaTime * gameTimeScale;

		foreach (var (forager, gatheringActivity, foodConsumer, gathererEntity)
		         in SystemAPI.Query<
			         RefRO<Forager>,
			         RefRW<GatheringActivity>,
			         RefRW<FoodConsumer>
			         >()
			         .WithEntityAccess())
		{
			var resourceEntity = gatheringActivity.ValueRW.ResourceEntity;

			var ripeBiomass = state.EntityManager.GetComponentData<RipeBiomass>(resourceEntity);

			const float EnergyDensity_kcalPerKg = 1000;
			float massCanGather = forager.ValueRO.GatheringSpeed * hoursDelta;
			float neededMass = foodConsumer.ValueRO.EnergyStillNeeded / EnergyDensity_kcalPerKg;

			float wantedMass = Math.Min(massCanGather, neededMass);
			float gatheredMass = Math.Min(wantedMass, ripeBiomass.Value);

			if (gatheredMass > 0) {
				ripeBiomass.Decrease(gatheredMass);
				state.EntityManager.SetComponentData(resourceEntity, ripeBiomass);

				foodConsumer.ValueRW.EnergyConsumedToday +=
					(uint) Mathf.RoundToInt(gatheredMass * EnergyDensity_kcalPerKg);
			}

			if (foodConsumer.ValueRO.IsSatiated || ripeBiomass.IsZero) {
				state.EntityManager.SetComponentEnabled<Activity>(gathererEntity, false);
				state.EntityManager.SetComponentEnabled<GatheringActivity>(gathererEntity, false);
			}
		}
	}
}



}
