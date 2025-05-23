using System;

using UnityEngine;

using Lib.Grid;



namespace Lib.VisualGrid {



/// <summary>
/// Hex grid with specific dimensions and geometry layout in 3D coordinate system.
/// </summary>
public class VisualRectangularHexMap3D
	: RectangularHexMap
{
	public HexLayout3D Layout { get; }


	//----------------------------------------------------------------------------------------------


	public VisualRectangularHexMap3D(HexLayout3D layout,
	                                 uint width, uint height,
	                                 HexMapLineOffset lineOffset)
		: base(width, height, layout.Orientation, lineOffset)
	{
		Layout = layout;
	}


	public VisualRectangularHexMap3D(HexLayout3D layout, RectangularHexMap map)
		: base(map.Width, map.Height, layout.Orientation, map.LineOffset)
	{
		if (layout.Orientation != map.Orientation)
			throw new ArgumentException();

		Layout = layout;
	}


	public AxialPosition? GetAxialPosition(Ray ray)
	{
		if (! Layout.GetAxialPosition(ray, out var freePos))
			return null;
		return IsInside(freePos) ? freePos : null;
	}
}



}
