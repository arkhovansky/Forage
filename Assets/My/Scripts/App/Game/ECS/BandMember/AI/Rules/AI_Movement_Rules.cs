using System.Collections.Generic;

using Lib.Grid;

using App.Game.ECS.BandMember.Movement.Rules;



namespace App.Game.ECS.BandMember.AI.Rules {



public record PathInfo(
	IReadOnlyList<AxialPosition> Path,
	float TotalMovementCost
);



public static class AI_Movement_Rules
{
	public static PathInfo CalculatePath(AxialPosition start, AxialPosition end,
	                                     Movement_Rules movementRules)
	{
		var path = HexGrid.GetLinearPath(start, end);

		return new PathInfo(path, path.Length * movementRules.MovementCost);
	}
}



}
