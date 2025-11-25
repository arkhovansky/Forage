using Lib.Grid;

using App.Game.ECS.BandMember.Energy.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.Gathering.Rules;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.BandMember.Movement.Rules;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;



namespace App.Game.ECS.BandMember.AI.Rules {



public static class AI_Foraging_Rules
{
	public static float GetForagingTime(PathInfo pathInfo, PhysicalMapParameters physicalMapParams, Walker walker,
	                                    RipeBiomass ripeBiomass, Gatherer gatherer, FoodConsumer foodConsumer,
	                                    Movement_Rules movementRules, Gathering_Rules gatheringRules)
	{
		float moveTime = movementRules.GetMovementTime(
			pathInfo.TotalMovementCost, physicalMapParams.TileInnerDiameter, walker.BaseSpeed_KmPerH);
		float gatherTime = gatheringRules.GetGatheringTime(
			foodConsumer.EnergyNeededPerDay,
			ripeBiomass.Value, physicalMapParams.CellArea, gatherer.GatheringSpeed);

		return GetForagingTime(moveTime, gatherTime);
	}


	public static float GetMinForagingTime(AxialPosition foragerPosition, AxialPosition resourcePosition,
	                                       PhysicalMapParameters physicalMapParams, Walker walker,
	                                       Gatherer gatherer, FoodConsumer foodConsumer,
	                                       Movement_Rules movementRules, Gathering_Rules gatheringRules)
	{
		float moveTime = movementRules.GetMinMovementTime(
			foragerPosition, resourcePosition, physicalMapParams.TileInnerDiameter, walker.BaseSpeed_KmPerH);
		float gatherTime = gatheringRules.GetMinGatheringTime(
			foodConsumer.EnergyNeededPerDay, gatherer.GatheringSpeed);

		return GetForagingTime(moveTime, gatherTime);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static float GetForagingTime(float moveTime, float gatherTime)
	{
		return moveTime * 2 + gatherTime;
	}
}



}
