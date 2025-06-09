using System;
using System.Collections.Generic;

using Unity.Transforms;
using UnityEngine;

using Lib.Grid;



namespace Lib.VisualGrid {



public enum FlatTopHexVertex
{
	Left,
	TopLeft,
	TopRight,
	Right,
	BottomRight,
	BottomLeft
}



public readonly struct HexOrientationData
{
	// Forward matrix
	public readonly float F0, F1, F2, F3;

	// Inverse matrix
	public readonly float B0, B1, B2, B3;


	public HexOrientationData(float f0, float f1, float f2, float f3,
	                          float b0, float b1, float b2, float b3)
	{
		F0 = f0; F1 = f1; F2 = f2; F3 = f3;
		B0 = b0; B1 = b1; B2 = b2; B3 = b3;
	}



	public static readonly HexOrientationData FlatTop = new HexOrientationData(
		// Axial and Cartesian systems have the same orientation (handedness)
		// 3f/2f, 0f, Mathf.Sqrt(3f) / 2f, Mathf.Sqrt(3f),
		// 2f/3f, 0f, -1f/3f, Mathf.Sqrt(3f) / 3f

		// Axial and Cartesian systems have different orientations (handedness)
		3f/2f, 0f, - Mathf.Sqrt(3f) / 2f, - Mathf.Sqrt(3f),
		2f/3f, 0f, -1f/3f, - Mathf.Sqrt(3f) / 3f
	);

	public static readonly HexOrientationData PointyTop = new HexOrientationData(
		// Axial and Cartesian systems have the same orientation (handedness)
		// Mathf.Sqrt(3f), Mathf.Sqrt(3f) / 2f, 0f, 3f/2f,
		// Mathf.Sqrt(3f) / 3f, -1f/3f, 0f, 2f/3f

		// Axial and Cartesian systems have different orientations (handedness)
		Mathf.Sqrt(3f), Mathf.Sqrt(3f) / 2f, 0f, -3f/2f,
		Mathf.Sqrt(3f) / 3f, 1f/3f, 0f, -2f/3f
	);
}



/// <summary>
/// Represents geometry of infinite hex grid in Cartesian coordinate system.
/// </summary>
/// <remarks>
/// Cartesian coordinate system has positive / standard / right-handed orientation (X -> Y counter-clockwise),
/// while grid's axial system is left-handed (Q -> R clockwise).
/// </remarks>
public struct HexLayout
{
	/// <summary>
	/// Cell orientation.
	/// </summary>
	public HexOrientation Orientation { get; }

	/// <summary>
	/// Coordinates of the center of the origin cell.
	/// </summary>
	public Vector2 Origin { get; }

	/// <summary>
	/// Scale factor.
	/// </summary>
	/// <remarks>
	/// Can be heterogeneous (different values for x and y).
	/// In the homogeneous case equals to the outer radius of a cell.
	/// </remarks>
	public Vector2 ScaleFactor { get; }

	/// <summary>
	/// Size of the cell bounding rectangle.
	/// </summary>
	public Vector2 CellSize { get; }

	/// <summary>
	/// Inner radius of a cell.
	/// </summary>
	public float InnerCellRadius { get; }

	/// <summary>
	/// Outer radius of a cell.
	/// </summary>
	public float OuterCellRadius { get; }

	/// <summary>
	/// Horizontal distance between adjacent hexagon centers.
	/// </summary>
	public float HorizontalSpacing { get; }

	/// <summary>
	/// Vertical distance between adjacent hexagon centers.
	/// </summary>
	public float VerticalSpacing { get; }



	private readonly HexOrientationData _orientationData;

	private readonly Vector2 _cellHalfSize;


	//----------------------------------------------------------------------------------------------


	private struct FractionalAxialPosition
	{
		public float Q, R;

		public FractionalAxialPosition(float q, float r) {
			Q = q; R = r;
		}

		public override string ToString()
			=> $"({Q}, {R})";
	}


	//----------------------------------------------------------------------------------------------


	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="orientation">The cell orientation.</param>
	/// <param name="scaleFactor">The scale factor.
	/// If not specified, set to (0.5, 0.5).</param>
	/// <param name="origin">Coordinates of the center of the origin cell.
	/// If not specified, set so that the left top of the origin cell's bounding rectangle is at the Cartesian origin.
	/// </param>
	public HexLayout(
		HexOrientation orientation,
		Vector2? scaleFactor = null,
		Vector2? origin = null
	)
	{
		Orientation = orientation;

		ScaleFactor = scaleFactor ?? new Vector2(.5f, .5f);
		CellSize = GetCellSize(orientation, ScaleFactor);
		Origin = origin ?? GetLeftTopOrigin(CellSize);

		_cellHalfSize = CellSize * 0.5f;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				_orientationData = HexOrientationData.FlatTop;

				InnerCellRadius = _cellHalfSize.y;
				OuterCellRadius = _cellHalfSize.x;

				HorizontalSpacing = 0.75f * CellSize.x;
				VerticalSpacing = CellSize.y;

				break;

			case HexOrientation.PointyTop:
				_orientationData = HexOrientationData.PointyTop;

				InnerCellRadius = _cellHalfSize.x;
				OuterCellRadius = _cellHalfSize.y;

				HorizontalSpacing = CellSize.x;
				VerticalSpacing = 0.75f * CellSize.y;

				break;

			default:
				throw new ArgumentOutOfRangeException();
		}
	}



	public readonly AxialPosition GetAxialPosition(Vector2 point)
	{
		var pt = new Vector2((point.x - Origin.x) / ScaleFactor.x,
			                 (point.y - Origin.y) / ScaleFactor.y);

		float q = _orientationData.B0 * pt.x + _orientationData.B1 * pt.y;
		float r = _orientationData.B2 * pt.x + _orientationData.B3 * pt.y;

		return Round(new FractionalAxialPosition(q, r));
	}


	public readonly Vector2 GetPoint(AxialPosition position)
	{
		Vector2 point;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				point.x = position.Q * HorizontalSpacing;
				point.y = -0.5f * (position.Q + 2 * position.R) * VerticalSpacing;

				break;

			case HexOrientation.PointyTop:
				point.x = 0.5f * (2 * position.Q + position.R) * HorizontalSpacing;
				point.y = -(position.R * VerticalSpacing);

				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		return point + Origin;
	}


	private readonly Vector2 GetPoint(FractionalAxialPosition position)
	{
		float x = _orientationData.F0 * position.Q + _orientationData.F1 * position.R;
		float y = _orientationData.F2 * position.Q + _orientationData.F3 * position.R;

		return new Vector2(x * ScaleFactor.x + Origin.x,
		                   y * ScaleFactor.y + Origin.y);
	}


	public readonly Vector2 GetLerpPoint(AxialPosition start, AxialPosition end, float t)
	{
		return GetPoint(
			Lerp(start, end, t));
	}


	public readonly Vector2 GetVertex(AxialPosition position, FlatTopHexVertex vertex)
	{
		return GetAbstractCellVertex(vertex) + GetPoint(position);
	}


	public readonly Vector2 GetAbstractCellVertex(FlatTopHexVertex vertex)
	{
		if (Orientation != HexOrientation.FlatTop)
			throw new InvalidOperationException("Orientation must be FlatTop");

		return vertex switch {
			FlatTopHexVertex.Left => new Vector2(-CellSize.x / 2, 0),
			FlatTopHexVertex.TopLeft => new Vector2(-0.25f * CellSize.x, CellSize.y / 2),
			FlatTopHexVertex.TopRight => new Vector2(0.25f * CellSize.x, CellSize.y / 2),
			FlatTopHexVertex.Right => new Vector2(CellSize.x / 2, 0),
			FlatTopHexVertex.BottomRight => new Vector2(0.25f * CellSize.x, -CellSize.y / 2),
			FlatTopHexVertex.BottomLeft => new Vector2(-0.25f * CellSize.x, -CellSize.y / 2),
			_ => throw new ArgumentOutOfRangeException(nameof(vertex), vertex, null)
		};
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
			path[i] = Round(Lerp(start, end, (float)(i+1) / distance));
		}
		path[distance - 1] = end;

		return path;
	}



	private static FractionalAxialPosition Lerp(AxialPosition start, AxialPosition end, float t)
	{
		return new FractionalAxialPosition(Lerp(start.Q, end.Q, t), Lerp(start.R, end.R, t));
	}

	private static float Lerp(int a, int b, float t)
	{
		return a + (b - a) * t;
	}



	private static AxialPosition Round(FractionalAxialPosition position)
	{
		float sFrac = - (position.Q + position.R);

		int q = Mathf.RoundToInt(position.Q);
		int r = Mathf.RoundToInt(position.R);
		int s = Mathf.RoundToInt(sFrac);

		float qDiff = Mathf.Abs(q - position.Q);
		float rDiff = Mathf.Abs(r - position.R);
		float sDiff = Mathf.Abs(s - sFrac);

		if (qDiff > rDiff && qDiff > sDiff) {
			q = -r - s;
		} else if (rDiff > sDiff) {
			r = -q - s;
		}

		return new AxialPosition(q, r);
	}



	public static Vector2 GetCellSize(HexOrientation orientation, Vector2 scaleFactor)
	{
		switch (orientation) {
			case HexOrientation.FlatTop:
				return new Vector2(2 * scaleFactor.x, Mathf.Sqrt(3) * scaleFactor.y);

			case HexOrientation.PointyTop:
				return new Vector2(Mathf.Sqrt(3) * scaleFactor.x, 2 * scaleFactor.y);

			default:
				throw new ArgumentOutOfRangeException();
		}
	}


	public static Vector2 GetLeftTopOrigin(Vector2 cellSize)
	{
		return new Vector2(cellSize.x / 2, -(cellSize.y / 2));
	}
}



}
