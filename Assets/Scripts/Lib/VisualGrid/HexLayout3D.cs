﻿using System;
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
	/// Inner radius of a cell.
	/// </summary>
	public readonly float InnerCellRadius
		=> _layout2D.InnerCellRadius;

	/// <summary>
	/// Outer radius of a cell.
	/// </summary>
	public readonly float OuterCellRadius
		=> _layout2D.OuterCellRadius;

	/// <summary>
	/// Horizontal distance between adjacent hexagon centers.
	/// </summary>
	public readonly float HorizontalSpacing
		=> _layout2D.HorizontalSpacing;

	/// <summary>
	/// Vertical distance between adjacent hexagon centers.
	/// </summary>
	public readonly float VerticalSpacing
		=> _layout2D.VerticalSpacing;

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


	public readonly Vector3 GetPoint(AxialPosition position)
	{
		return ProjectionMatrix.ProjectPoint(
			_layout2D.GetPoint(position));
	}


	public readonly Vector3 GetLerpPoint(AxialPosition start, AxialPosition end, float t)
	{
		return ProjectionMatrix.ProjectPoint(
			_layout2D.GetLerpPoint(start, end, t));
	}


	public readonly Vector3 GetVertex(AxialPosition position, FlatTopHexVertex vertex)
	{
		return ProjectionMatrix.ProjectPoint(
			_layout2D.GetVertex(position, vertex));
	}



	/// <summary>
	/// Get cell mesh.
	/// </summary>
	/// <remarks>
	/// The mesh center is at the coordinate system origin.
	/// </remarks>
	/// <returns>The cell mesh.</returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public readonly Mesh GetCellMesh()
	{
		Vector2[] vertices2;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				vertices2 = new [] {
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.Left),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.TopLeft),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.TopRight),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.Right),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.BottomRight),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.BottomLeft),
					new (0, 0)
				};

				break;

			case HexOrientation.PointyTop:
				throw new NotImplementedException();

			default:
				throw new ArgumentOutOfRangeException();
		}

		var projectionMatrix = ProjectionMatrix;

		var mesh = new Mesh {
			vertices = Array.ConvertAll(vertices2, v => projectionMatrix.ProjectPoint(v)),
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


	/// <summary>
	/// Get the list of cell border vertices.
	/// </summary>
	/// <remarks>
	/// The cell center is at the coordinate system origin.
	/// </remarks>
	/// <returns>The list of cell border vertices in clockwise order.</returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public readonly IReadOnlyList<Vector3> GetCellBorderVertices()
	{
		Vector2[] vertices2;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				vertices2 = new[] {
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.Left),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.TopLeft),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.TopRight),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.Right),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.BottomRight),
					_layout2D.GetAbstractCellVertex(FlatTopHexVertex.BottomLeft)
				};

				break;

			case HexOrientation.PointyTop:
				throw new NotImplementedException();

			default:
				throw new ArgumentOutOfRangeException();
		}

		var projectionMatrix = ProjectionMatrix;
		return Array.ConvertAll(vertices2, v => projectionMatrix.ProjectPoint(v));
	}


	public readonly LocalTransform GetCellLocalTransform(AxialPosition position)
	{
		return LocalTransform.FromPosition(
			GetPoint(position));
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
