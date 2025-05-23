using System;
using System.Collections.Generic;

using Unity.Transforms;
using UnityEngine;

using Lib.Grid;
using Lib.Math;



namespace Lib.VisualGrid {



/// <summary>
/// Represents geometry of infinite hex grid in 3D coordinate system.
/// </summary>
public struct HexLayout3D
{
	/// <summary>
	/// Cell orientation.
	/// </summary>
	public readonly HexOrientation Orientation
		=> _layout2D.Orientation;

	/// <summary>
	/// Coordinates of the center of the origin cell.
	/// </summary>
	public Vector3 Origin { get; }

	/// <summary>
	/// Scale factor.
	/// </summary>
	/// <remarks>
	/// Can be heterogeneous (different values for x and y).
	/// In the homogeneous case equals to the outer radius of a cell.
	/// </remarks>
	public readonly Vector2 ScaleFactor
		=> _layout2D.ScaleFactor;

	/// <summary>
	/// Size of the cell bounding rectangle.
	/// </summary>
	public readonly Vector2 CellSize
		=> _layout2D.CellSize;

	/// <summary>
	/// Matrix for projecting from 2D to 3D and back.
	/// </summary>
	public Matrix3x2 ProjectionMatrix { get; }


	//----------------------------------------------------------------------------------------------


	private readonly HexLayout _layout2D;

	private readonly Plane _plane;


	//----------------------------------------------------------------------------------------------


	public HexLayout3D(HexLayout layout2D, Matrix3x2 projectionMatrix)
	{
		_layout2D = layout2D;
		ProjectionMatrix = projectionMatrix;
		Origin = projectionMatrix.ProjectPoint(layout2D.Origin);
		_plane = new Plane(projectionMatrix.IBasisVector, Vector3.zero, projectionMatrix.JBasisVector);
	}



	public readonly Mesh GetOriginCellMesh()
	{
		Vector2[] vertices2;  // Origin at the center

		switch (Orientation) {
			case HexOrientation.FlatTop:
				vertices2 = new Vector2[] {
					new (-CellSize.x / 2, 0),
					new (-0.25f * CellSize.x, CellSize.y / 2),
					new (0.25f * CellSize.x, CellSize.y / 2),
					new (CellSize.x / 2, 0),
					new (0.25f * CellSize.x, -CellSize.y / 2),
					new (-0.25f * CellSize.x, -CellSize.y / 2),
					new (0, 0)
				};

				break;

			case HexOrientation.PointyTop:
				throw new NotImplementedException();

			default:
				throw new ArgumentOutOfRangeException();
		}

		var projectionMatrix = ProjectionMatrix;
		var origin2D = _layout2D.Origin;

		var mesh = new Mesh {
			vertices = Array.ConvertAll(vertices2, v => projectionMatrix.ProjectPoint(v + origin2D)),
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
					new (-CellSize.x / 2, 0),
					new (-0.25f * CellSize.x, CellSize.y / 2),
					new (0.25f * CellSize.x, CellSize.y / 2),
					new (CellSize.x / 2, 0),
					new (0.25f * CellSize.x, -CellSize.y / 2),
					new (-0.25f * CellSize.x, -CellSize.y / 2)
				};

				break;

			case HexOrientation.PointyTop:
				throw new NotImplementedException();

			default:
				throw new ArgumentOutOfRangeException();
		}

		var projectionMatrix = ProjectionMatrix;
		var origin2D = _layout2D.Origin;
		return Array.ConvertAll(vertices2, v => projectionMatrix.ProjectPoint(v + origin2D));
	}


	public readonly Vector3 GetOriginCellCenter()
	{
		return Origin;
	}


	public readonly LocalTransform GetCellLocalTransform(AxialPosition position)
	{
		Vector2 point2 = _layout2D.GetPoint(position) - _layout2D.Origin;
		Vector3 point3 = ProjectionMatrix.ProjectPoint(point2);
		return LocalTransform.FromPosition(point3);
	}


	public readonly bool GetAxialPosition(Ray ray, out AxialPosition position)
	{
		if (!_plane.Raycast(ray, out float enter)) {
			position = default;
			return false;
		}

		Vector3 point3 = ray.GetPoint(enter);

		// Point is already on the grid plane, so this is just a coordinate conversion
		Vector2 point2 = ProjectionMatrix.BackProjectPoint(point3);

		position = _layout2D.GetAxialPosition(point2);
		return true;
	}
}



}
