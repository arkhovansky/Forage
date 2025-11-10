using App.Game.ECS.Resource.Plant.Components;



namespace App.Game.ECS.Resource.Plant.Rules {



public static class PlantCycle_Rules
{
	public static void UpdateRipeBiomass(ref RipeBiomass ripeBiomass,
	                                     PlantResource resource,
	                                     YearPeriod yearPeriod)
	{
			if (resource.RipenessPeriod == yearPeriod)
				ripeBiomass.Reset(resource.PotentialBiomass);
			else
				ripeBiomass.Reset(0);
	}
}



}
