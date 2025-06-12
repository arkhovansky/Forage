using System;

using UnityEngine;

using Lib.Grid;



namespace Lib.VisualGrid {



/// <summary>
/// Hex grid with specific dimensions and geometry layout.
/// </summary>
public class VisualRectangularHexMap
	: RectangularHexMap
{
	public HexLayout Layout { get; }


	//----------------------------------------------------------------------------------------------


	public VisualRectangularHexMap(HexLayout layout,
	                               uint width, uint height,
	                               HexMapLineOffset lineOffset)
		: base(width, height, layout.Orientation, lineOffset)
	{
		Layout = layout;
	}


	public VisualRectangularHexMap(HexLayout layout, RectangularHexMap map)
		: base(map.Width, map.Height, layout.Orientation, map.LineOffset)
	{
		if (layout.Orientation != map.Orientation)
			throw new ArgumentException();

		Layout = layout;
	}


	public AxialPosition? GetAxialPosition(Vector2 point)
	{
		AxialPosition freePos = Layout.GetAxialPosition(point);
		return Contains(freePos) ? freePos : null;
	}
}



}
