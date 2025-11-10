using System.Collections.Generic;

using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.BandMember.Movement.Rules;



namespace App.Game.ECS.BandMember.AI.Rules {



public record PathInfo(
	IReadOnlyList<AxialPosition> Path,
	float TotalMovementCost
);



public static class AI_Movement_Rules
{
	public static PathInfo CalculatePath(AxialPosition start, AxialPosition end)
	{
		var path = HexLayout.GetLinearPath(start, end);

		return new PathInfo(path, path.Length * Movement_Rules.MovementCost);
	}
}



}
