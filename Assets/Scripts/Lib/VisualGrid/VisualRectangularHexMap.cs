using System;

using UnityEngine;

using Lib.Grid;



namespace Lib.VisualGrid {



/// <summary>
/// Hex grid with specific dimensions and geometry layout.
/// </summary>
public struct VisualRectangularHexMap
{
	public RectangularHexMap Map { get; }

	public HexLayout Layout { get; }


	//----------------------------------------------------------------------------------------------


	public VisualRectangularHexMap(RectangularHexMap map, HexLayout layout)
	{
		if (layout.Orientation != map.Orientation)
			throw new ArgumentException();

		Map = map;
		Layout = layout;
	}


	public readonly AxialPosition? GetAxialPosition(Vector2 point)
	{
		AxialPosition freePos = Layout.GetAxialPosition(point);
		return Map.Contains(freePos) ? freePos : null;
	}
}



}
