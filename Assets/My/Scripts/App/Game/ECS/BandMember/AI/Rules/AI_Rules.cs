using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Resource.Plant.Components;



namespace App.Game.ECS.BandMember.AI.Rules {



public static class AI_Rules
{
	public static Goal SelectGoal(bool isDaylight, FoodConsumer foodConsumer)
	{
		if (isDaylight) {
			return foodConsumer.IsSatiated ? Goal.Leisure : Goal.Forage;
		}
		else
			return Goal.Sleep;
	}


	public static bool Should_GatherOnTile(FoodConsumer foodConsumer, RipeBiomass ripeBiomass)
	{
		return !foodConsumer.IsSatiated && !ripeBiomass.IsZero;
	}


	public static bool Should_Leisure(bool isDaylight)
	{
		return isDaylight;
	}

	public static bool Should_Sleep(bool isDaylight)
	{
		return !isDaylight;
	}
}



}
