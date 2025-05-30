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


	public float GeometricWidth {
		get {
			return Orientation switch {
				HexOrientation.FlatTop => Layout.HorizontalSpacing * (Width - 1) + Layout.CellSize.x,
				HexOrientation.PointyTop => Layout.CellSize.x * (Width + 0.5f),
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}

	public float GeometricHeight {
		get {
			return Orientation switch {
				HexOrientation.FlatTop => Layout.CellSize.y * (Height + 0.5f),
				HexOrientation.PointyTop => Layout.VerticalSpacing * (Height - 1) + Layout.CellSize.y,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}

	public Vector3 GeometricCenter {
		get {
			var point2D = new Vector2(Layout.Origin.x - Layout.CellSize.x / 2 + GeometricWidth / 2,
			                          Layout.Origin.y - Layout.CellSize.y / 2 + GeometricHeight / 2);
			return Layout.ProjectionMatrix.ProjectPoint(point2D);
		}
	}


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
