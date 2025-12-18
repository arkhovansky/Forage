using System.Collections.Generic;



namespace Lib.Grid {



public struct HexGrid
{
	public HexOrientation Orientation { get; }


	//----------------------------------------------------------------------------------------------


	public static readonly AxialVector[] AxialDirectionVectors = {
		new(+1, 0), new(+1, -1), new(0, -1), new(-1, 0), new(-1, +1), new(0, +1)
	};


	//----------------------------------------------------------------------------------------------


	public HexGrid(HexOrientation orientation)
	{
		Orientation = orientation;
	}


	public static AxialPosition Neighbor(AxialPosition cell, int directionIndex)
	{
		return cell + AxialDirectionVectors[directionIndex];
	}


	public static uint Distance(AxialPosition start, AxialPosition end)
	{
		var vec = end - start;
		return (uint) (System.Math.Abs(vec.Q) + System.Math.Abs(vec.Q + vec.R) + System.Math.Abs(vec.R)) / 2;
	}


	public static AxialPosition[] GetLinearPath(AxialPosition start, AxialPosition end)
	{
		var distance = Distance(start, end);
		var path = new AxialPosition[distance];

		if (distance == 0)
			return path;

		for (var i = 0; i < distance - 1; i++) {
			path[i] = Fractional_AxialPosition.Lerp(start, end, (float)(i+1) / distance).Round();
		}
		path[distance - 1] = end;

		return path;
	}


	public readonly IReadOnlyList<AxialPosition> GetCellsWithinRadius(AxialPosition center, uint radius)
	{
		// r c          n  total
		// 0 1          1
		// 1 2          6   7
		// 2 3  6+2*3 =12  19
		// 3 4  8+2*5 =18  37

		// 1 + 6 * r*(r+1)/2

		// ReSharper disable once InconsistentNaming
		var R = (int) radius;

		var cellCount = 1 + 6 * (R * (R + 1) / 2);
		var cellPositions = new AxialPosition[cellCount];

		var i = 0;
		for (var q = -R; q <= R; ++q)
		for (var r = System.Math.Max(-R, -q - R); r <= System.Math.Min(R, -q + R); ++r)
			cellPositions[i++] = center + new AxialVector(q, r);

		return cellPositions;
	}
}



}
