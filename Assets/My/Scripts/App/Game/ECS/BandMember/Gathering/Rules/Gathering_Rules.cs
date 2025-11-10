using System;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.Resource.Plant.Components;



namespace App.Game.ECS.BandMember.Gathering.Rules {



public static class Gathering_Rules
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



	public static void Gather(ref RipeBiomass ripeBiomass,
	                          ref FoodConsumer foodConsumer,
	                          Gatherer gatherer,
	                          float hoursDelta,
	                          float innerCellDiameter)
	{
		float gatheringSpeed =
			GetGatheringSpeed(ripeBiomass.Value, innerCellDiameter, gatherer.GatheringSpeed);

		float massCanGather = gatheringSpeed * hoursDelta;
		float neededMass = foodConsumer.EnergyStillNeeded / EnergyDensity_KcalPerKg;

		float wantedMass = Math.Min(massCanGather, neededMass);
		float gatheredMass = Math.Min(wantedMass, ripeBiomass.Value);

		if (gatheredMass > 0) {
			ripeBiomass.Decrease(gatheredMass);
			foodConsumer.ConsumeEnergy(gatheredMass * EnergyDensity_KcalPerKg);
		}
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


	//----------------------------------------------------------------------------------------------
	// private


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
}



}
