using System;

using UnityEngine;



namespace Lib.Grid.Spatial {



/// <summary>
/// Hex grid with specific dimensions and geometry layout in 3D coordinate system.
/// </summary>
public struct Spatial_RectangularHexMap_3D
{
	public RectangularHexMap Map { get; }

	public HexGridLayout_3D Layout { get; }


	public readonly float GeometricWidth {
		get {
			return Map.Orientation switch {
				HexOrientation.FlatTop => Layout.HorizontalSpacing * (Map.Width - 1) + Layout.CellSize.x,
				HexOrientation.PointyTop => Layout.CellSize.x * (Map.Width + 0.5f),
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}

	public readonly float GeometricHeight {
		get {
			return Map.Orientation switch {
				HexOrientation.FlatTop => Layout.CellSize.y * (Map.Height + 0.5f),
				HexOrientation.PointyTop => Layout.VerticalSpacing * (Map.Height - 1) + Layout.CellSize.y,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}

	public readonly Rect BoundingRect2D {
		get {
			float x, y;

			switch (Map.Orientation) {
				case HexOrientation.FlatTop:
					x = Layout.Origin.x - Layout.CellSize.x / 2;
					y = Map.LineOffset switch {
						HexMapLineOffset.Odd => Layout.Origin.y + Layout.CellSize.y / 2,
						HexMapLineOffset.Even => Layout.Origin.y + Layout.CellSize.y,
						_ => throw new ArgumentOutOfRangeException()
					};
					break;
				case HexOrientation.PointyTop:
					x = Map.LineOffset switch {
						HexMapLineOffset.Odd => Layout.Origin.x - Layout.CellSize.x / 2,
						HexMapLineOffset.Even => Layout.Origin.x - Layout.CellSize.x,
						_ => throw new ArgumentOutOfRangeException()
					};
					y = Layout.Origin.y + Layout.CellSize.y / 2;
					break;
				default: throw new ArgumentOutOfRangeException();
			};

			return new Rect(x, y, GeometricWidth, GeometricHeight);
		}
	}

	public readonly Vector3 GeometricCenter
		=> Layout.ProjectionMatrix.ProjectPoint(BoundingRect2D.center);


	//----------------------------------------------------------------------------------------------


	public Spatial_RectangularHexMap_3D(RectangularHexMap map, HexGridLayout_3D layout)
	{
		if (layout.Orientation != map.Orientation)
			throw new ArgumentException();

		Map = map;
		Layout = layout;
	}


	public readonly AxialPosition? GetAxialPosition(Ray ray)
	{
		if (! Layout.GetAxialPosition(ray, out var freePos))
			return null;
		return Map.Contains(freePos) ? freePos : null;
	}


	public readonly Vector3 GetVertex(OffsetPosition offset, FlatTopHexVertex vertex)
	{
		return Layout.GetVertex(Map.AxialPositionFrom(offset), vertex);
	}
}



}
