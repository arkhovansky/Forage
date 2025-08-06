using System;

using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Gathering {



[UpdateInGroup(typeof(DomainSimulation))]
public partial struct Gathering_System : ISystem
{
	private const float EnergyDensity_KcalPerKg = 1000;

	/// <summary>
	/// The biomass density (kg/km^2) that is considered rich.
	/// </summary>
	/// <remarks>
	/// If density is larger, gathering speed equals to the base one.
	/// If density is smaller, gathering speed is decreased.
	/// </remarks>
	private const float RichBiomassDensity = 1000;

	private const float MinGatheringSpeedCoef = 0.2f;



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
			         Components.Gatherer,
			         Components.GatheringActivity,
			         EnabledRefRW<Components.GatheringActivity>, EnabledRefRW<Activity>,
			         RefRW<FoodConsumer>
			         >())
		{
			var ripeBiomass = SystemAPI.GetComponentRW<RipeBiomass>(gatheringActivity.ResourceEntity);

			float gatheringSpeed =
				GetGatheringSpeed(ripeBiomass.ValueRO.Value, innerCellDiameter, gatherer.GatheringSpeed);

			float massCanGather = gatheringSpeed * hoursDelta;
			float neededMass = foodConsumer.ValueRO.EnergyStillNeeded / EnergyDensity_KcalPerKg;

			float wantedMass = Math.Min(massCanGather, neededMass);
			float gatheredMass = Math.Min(wantedMass, ripeBiomass.ValueRO.Value);

			if (gatheredMass > 0) {
				ripeBiomass.ValueRW.Decrease(gatheredMass);
				foodConsumer.ValueRW.ConsumeEnergy(gatheredMass * EnergyDensity_KcalPerKg);
			}


			// Possibly stop activity
			if (foodConsumer.ValueRO.IsSatiated || ripeBiomass.ValueRO.IsZero) {
				activityEnabled.ValueRW = false;
				gatheringActivityEnabled.ValueRW = false;
			}
		}
	}



	private static float GetGatheringSpeed(float ripeBiomass, float cellArea, float baseGatheringSpeed)
	{
		float biomassDensity = ripeBiomass / cellArea;

		if (biomassDensity >= RichBiomassDensity)
			return baseGatheringSpeed;
		else {
			var x = 1 - biomassDensity / RichBiomassDensity;
			var speedCoef = 1 - x*x + MinGatheringSpeedCoef * x;
			return baseGatheringSpeed * speedCoef;
		}
	}


	private static float GetGatheringTime(float neededEnergy, float gatheringSpeed)
	{
		float mass = neededEnergy / EnergyDensity_KcalPerKg;
		return mass / gatheringSpeed;
	}


	public static float GetGatheringTime(float neededEnergy,
	                                     float ripeBiomass, float cellArea, float baseGatheringSpeed)
	{
		return GetGatheringTime(neededEnergy,
		                        GetGatheringSpeed(ripeBiomass, cellArea, baseGatheringSpeed));
	}

	public static float GetMinGatheringTime(float neededEnergy, float baseGatheringSpeed)
	{
		return GetGatheringTime(neededEnergy, baseGatheringSpeed);
	}
}



}
