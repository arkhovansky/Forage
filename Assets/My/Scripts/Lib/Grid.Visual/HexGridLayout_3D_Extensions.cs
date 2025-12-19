using System;

using UnityEngine;

using Lib.Grid.Spatial;



namespace Lib.Grid.Visual {



public static class HexGridLayout_3D_Extensions
{
	/// <summary>
	/// Get cell mesh.
	/// </summary>
	/// <remarks>
	/// The mesh center is at the coordinate system origin.
	/// </remarks>
	/// <returns>The cell mesh.</returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public static Mesh GetCellMesh(this in HexGridLayout_3D self)
	{
		Vector2[] vertices2;

		var layout = self.Layout_2D;

		switch (self.Orientation) {
			case HexOrientation.FlatTop:
				vertices2 = new [] {
					layout.GetAbstractCellVertex(FlatTopHexVertex.Left),
					layout.GetAbstractCellVertex(FlatTopHexVertex.TopLeft),
					layout.GetAbstractCellVertex(FlatTopHexVertex.TopRight),
					layout.GetAbstractCellVertex(FlatTopHexVertex.Right),
					layout.GetAbstractCellVertex(FlatTopHexVertex.BottomRight),
					layout.GetAbstractCellVertex(FlatTopHexVertex.BottomLeft),
					new (0, 0)
				};

				break;

			case HexOrientation.PointyTop:
				throw new NotImplementedException();

			default:
				throw new ArgumentOutOfRangeException();
		}

		var projectionMatrix = self.ProjectionMatrix;

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
}



}
