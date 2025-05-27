using System;
using System.Collections.Generic;

using Unity.Transforms;
using UnityEngine;

using Lib.Grid;



namespace Lib.VisualGrid {



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



	private readonly HexOrientationData _orientationData;

	private readonly Vector2 _cellHalfSize;

	private readonly float _horizontalSpacing;
	private readonly float _verticalSpacing;


	//----------------------------------------------------------------------------------------------


	private struct FractionalCubePosition
	{
		public float Q, R, S;

		public FractionalCubePosition(float q, float r, float s) {
			Q = q; R = r; S = s;
		}

		public FractionalCubePosition(float q, float r) {
			Q = q; R = r; S = -q - r;
		}

		public override string ToString()
			=> $"({Q}, {R}, {S})";
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

				_horizontalSpacing = 0.75f * CellSize.x;
				_verticalSpacing = CellSize.y;

				break;

			case HexOrientation.PointyTop:
				_orientationData = HexOrientationData.PointyTop;

				_horizontalSpacing = CellSize.x;
				_verticalSpacing = 0.75f * CellSize.y;

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

		var fCubePos = new FractionalCubePosition(q, r, -q - r);

		return Round(fCubePos);
	}


	public readonly Vector2 GetPoint(AxialPosition position)
	{
		Vector2 point;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				point.x = position.Q * _horizontalSpacing;
				point.y = -0.5f * (position.Q + 2 * position.R) * _verticalSpacing;

				break;

			case HexOrientation.PointyTop:
				point.x = 0.5f * (2 * position.Q + position.R) * _horizontalSpacing;
				point.y = -(position.R * _verticalSpacing);

				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		return point + Origin;
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



	private static FractionalCubePosition Lerp(AxialPosition start, AxialPosition end, float t)
	{
		return new FractionalCubePosition(Lerp(start.Q, end.Q, t), Lerp(start.R, end.R, t));
	}

	private static float Lerp(int a, int b, float t)
	{
		return a + (b - a) * t;
	}



	private static AxialPosition Round(FractionalCubePosition fCubePos)
	{
		int q = (int) Mathf.Round(fCubePos.Q);
		int r = (int) Mathf.Round(fCubePos.R);
		int s = (int) Mathf.Round(fCubePos.S);

		float qDiff = Mathf.Abs(q - fCubePos.Q);
		float rDiff = Mathf.Abs(r - fCubePos.R);
		float sDiff = Mathf.Abs(s - fCubePos.S);

		if (qDiff > rDiff && qDiff > sDiff) {
			q = -r - s;
		} else if (rDiff > sDiff) {
			r = -q - s;
		}
		// else {
		// 	s = -q - r;
		// }

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
