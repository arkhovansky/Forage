using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.BandMember.Movement.Components;



namespace App.Game.ECS.BandMember.Movement.Rules {



public static class Movement_Rules
{
	public const float MovementCost = 1.5f;

	private const float MinMovementCost = MovementCost;


	/// <summary>
	/// Move entity inside a cell that is a path destination
	/// </summary>
	/// <param name="intraCellMovement"></param>
	/// <param name="hoursDelta"></param>
	/// <param name="cellPhysicalInnerDiameter"></param>
	/// <param name="walker"></param>
	public static void MoveInsideDestinationCell(ref IntraCellMovement intraCellMovement,
	                                             ref float hoursDelta,
	                                             float cellPhysicalInnerDiameter,
	                                             Walker walker)
	{
		var distanceToCellCenter = intraCellMovement.DistanceToCenter * cellPhysicalInnerDiameter;
		var speed = walker.BaseSpeed_KmPerH / MovementCost;
		var hoursToCellCenter = distanceToCellCenter / speed;

		if (hoursDelta > hoursToCellCenter) {  // Reached cell center and beyond
			intraCellMovement.SetAtCenter();

			hoursDelta -= hoursToCellCenter;
		}
		else {  // Not reached cell center
			var distance = hoursDelta * speed;
			intraCellMovement.Advance(distance / cellPhysicalInnerDiameter);

			hoursDelta = 0;
		}
	}


	/// <summary>
	/// Move entity inside a cell that is transit
	/// </summary>
	/// <param name="intraCellMovement"></param>
	/// <param name="hoursDelta"></param>
	/// <param name="cellPhysicalInnerDiameter"></param>
	/// <param name="walker"></param>
	/// <param name="cellPosition"></param>
	/// <returns>Boolean indicating if entity has advanced to the next cell</returns>
	public static bool MoveInsideTransitCell(ref IntraCellMovement intraCellMovement,
	                                         ref float hoursDelta,
	                                         float cellPhysicalInnerDiameter,
	                                         Walker walker,
	                                         AxialPosition cellPosition)
	{
		var distanceToCellEdge = intraCellMovement.DistanceToFinalEdge * cellPhysicalInnerDiameter;
		var speed = walker.BaseSpeed_KmPerH / MovementCost;
		var hoursToCellEdge = distanceToCellEdge / speed;

		if (hoursDelta > hoursToCellEdge) {  // Reached cell edge and beyond
			intraCellMovement.SetAtStartEdge(previousPosition: cellPosition);

			hoursDelta -= hoursToCellEdge;

			return true;
		}
		else {  // Not reached cell edge
			var distance = hoursDelta * speed;
			intraCellMovement.Advance(distance / cellPhysicalInnerDiameter);

			hoursDelta = 0;

			return false;
		}
	}



	public static float GetMovementTime(float pathTotalMovementCost, float tilePhysicalInnerDiameter, float baseSpeed)
	{
		return (tilePhysicalInnerDiameter / baseSpeed) * pathTotalMovementCost;
	}

	public static float GetMinMovementTime(AxialPosition foragerPosition, AxialPosition resourcePosition,
	                                       float tilePhysicalInnerDiameter,
	                                       float baseSpeed)
	{
		var pathTileCount = HexLayout.Distance(foragerPosition, resourcePosition);
		return GetMovementTime(pathTileCount * MinMovementCost, tilePhysicalInnerDiameter, baseSpeed);
	}
}



}
