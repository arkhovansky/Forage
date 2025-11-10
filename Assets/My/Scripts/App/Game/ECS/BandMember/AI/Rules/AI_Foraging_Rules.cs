using Lib.Grid;

using App.Game.ECS.BandMember.Components;
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
	                                    RipeBiomass ripeBiomass, Gatherer gatherer, FoodConsumer foodConsumer)
	{
		float moveTime = Movement_Rules.GetMovementTime(
			pathInfo.TotalMovementCost, physicalMapParams.TileInnerDiameter, walker.BaseSpeed_KmPerH);
		float gatherTime = Gathering_Rules.GetGatheringTime(
			foodConsumer.EnergyNeededPerDay,
			ripeBiomass.Value, physicalMapParams.CellArea, gatherer.GatheringSpeed);

		return GetForagingTime(moveTime, gatherTime);
	}


	public static float GetMinForagingTime(AxialPosition foragerPosition, AxialPosition resourcePosition,
	                                       PhysicalMapParameters physicalMapParams, Walker walker,
	                                       Gatherer gatherer, FoodConsumer foodConsumer)
	{
		float moveTime = Movement_Rules.GetMinMovementTime(
			foragerPosition, resourcePosition, physicalMapParams.TileInnerDiameter, walker.BaseSpeed_KmPerH);
		float gatherTime = Gathering_Rules.GetMinGatheringTime(
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
