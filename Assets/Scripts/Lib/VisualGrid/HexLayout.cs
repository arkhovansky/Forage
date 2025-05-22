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
		3.0f / 2.0f, 0.0f, Mathf.Sqrt(3.0f) / 2.0f, Mathf.Sqrt(3.0f),
		2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Mathf.Sqrt(3.0f) / 3.0f);

	public static readonly HexOrientationData PointyTop = new HexOrientationData(
		Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
		Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f);
}



public struct HexLayout
{
	public HexOrientation Orientation { get; }

	public Vector2 Origin { get; }

	public Vector2 ScaleFactor { get; }

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

		public override string ToString()
			=> $"({Q}, {R}, {S})";
	}


	//----------------------------------------------------------------------------------------------


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



	public AxialPosition GetAxialPosition(Vector2 point)
	{
		// The Y axes of the grid and the input point are opposite, hence '-' in Y expression
		var pt = new Vector2((point.x - Origin.x) / ScaleFactor.x,
			                 -(point.y - Origin.y) / ScaleFactor.y);

		float q = _orientationData.B0 * pt.x + _orientationData.B1 * pt.y;
		float r = _orientationData.B2 * pt.x + _orientationData.B3 * pt.y;

		var fCubePos = new FractionalCubePosition(q, r, -q - r);

		return Round(fCubePos);
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



	public readonly Mesh GetOriginCellMesh()
	{
		Vector2[] vertices2;  // Origin at the center

		switch (Orientation) {
			case HexOrientation.FlatTop:
				vertices2 = new Vector2[] {
					new (-_cellHalfSize.x, 0),
					new (-0.25f * CellSize.x, _cellHalfSize.y),
					new (0.25f * CellSize.x, _cellHalfSize.y),
					new (_cellHalfSize.x, 0),
					new (0.25f * CellSize.x, -_cellHalfSize.y),
					new (-0.25f * CellSize.x, -_cellHalfSize.y),
					new (0, 0)
				};

				break;

			case HexOrientation.PointyTop:
				throw new NotImplementedException();

			default:
				throw new ArgumentOutOfRangeException();
		}

		var origin = Origin;

		var mesh = new Mesh {
			vertices = Array.ConvertAll(vertices2, v => new Vector3(v.x + origin.x, 0, v.y + origin.y)),
			triangles = new[] {
				0, 1, 6,
				1, 2, 6,
				2, 3, 6,
				3, 4, 6,
				4, 5, 6,
				5, 0, 6
			}
		};

		mesh.RecalculateNormals();

		return mesh;
	}



	public readonly IReadOnlyList<Vector3> GetOriginCellBorderVertices()
	{
		Vector2[] vertices2;  // Origin at the center

		switch (Orientation) {
			case HexOrientation.FlatTop:
				vertices2 = new Vector2[] {
					new (-_cellHalfSize.x, 0),
					new (-0.25f * CellSize.x, _cellHalfSize.y),
					new (0.25f * CellSize.x, _cellHalfSize.y),
					new (_cellHalfSize.x, 0),
					new (0.25f * CellSize.x, -_cellHalfSize.y),
					new (-0.25f * CellSize.x, -_cellHalfSize.y)
				};

				break;

			case HexOrientation.PointyTop:
				throw new NotImplementedException();

			default:
				throw new ArgumentOutOfRangeException();
		}

		var origin = Origin;
		return Array.ConvertAll(vertices2, v => new Vector3(v.x + origin.x, 0, v.y + origin.y));
	}



	public readonly Vector3 GetOriginCellCenter()
	{
		return new Vector3(Origin.x, 0, Origin.y);
	}



	public readonly LocalTransform GetCellLocalTransform(AxialPosition position)
	{
		float x, z;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				x = position.Q * _horizontalSpacing;
				z = -0.5f * (position.Q + 2 * position.R) * _verticalSpacing;

				break;

			case HexOrientation.PointyTop:
				x = 0.5f * (2 * position.Q + position.R) * _horizontalSpacing;
				z = -(position.R * _verticalSpacing);

				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		return LocalTransform.FromPosition(x, 0, z);
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
