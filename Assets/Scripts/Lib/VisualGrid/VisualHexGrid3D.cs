using System;

using UnityEngine;

using Lib.Grid;



namespace Lib.VisualGrid {



/// <summary>
/// Hex grid with specific dimensions and geometry layout in 3D coordinate system.
/// </summary>
public class VisualHexGrid3D
	: HexGrid
{
	public HexLayout3D Layout { get; }


	//----------------------------------------------------------------------------------------------


	public VisualHexGrid3D(HexLayout3D layout,
	                       uint width, uint height,
	                       HexGridLineOffset lineOffset)
		: base(width, height, layout.Orientation, lineOffset)
	{
		Layout = layout;
	}


	public VisualHexGrid3D(HexLayout3D layout, HexGrid grid)
		: base(grid.Width, grid.Height, layout.Orientation, grid.LineOffset)
	{
		if (layout.Orientation != grid.Orientation)
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
