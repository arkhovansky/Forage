using System.Collections.Generic;



namespace Lib.Grid {



public struct HexGrid
{
	public HexOrientation Orientation { get; }



	public HexGrid(HexOrientation orientation)
	{
		Orientation = orientation;
	}


	public IReadOnlyList<AxialPosition> GetCellsWithinRadius(AxialPosition center, uint radius)
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
			cellPositions[i++] = center + new AxialPosition(q, r);

		return cellPositions;
	}
}



}
