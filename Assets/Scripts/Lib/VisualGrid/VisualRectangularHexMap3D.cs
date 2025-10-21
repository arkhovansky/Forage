using System;

using UnityEngine;
using UnityEngine.Assertions;

using Lib.Grid;



namespace Lib.VisualGrid {



/// <summary>
/// Hex grid with specific dimensions and geometry layout in 3D coordinate system.
/// </summary>
public struct VisualRectangularHexMap3D
{
	public RectangularHexMap Map { get; }

	public HexLayout3D Layout { get; }


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


	public VisualRectangularHexMap3D(RectangularHexMap map, HexLayout3D layout)
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



	public readonly Mesh GetGridLinesMesh()
	{
		int oddColumnCount = (int) (Map.Width / 2 + Map.Width % 2);
		int evenColumnCount = (int) (Map.Width - oddColumnCount);
		int outsideEvenColumnCount = (int) (1 - Map.Width % 2);
		int insideEvenColumnCount = evenColumnCount - outsideEvenColumnCount;


		// Vertices

		int oddColumnVertexCount = (int) (2 + 4 * Map.Height);
		const int evenColumnVertexCount = 2;
		int outsideEvenColumnRightVertexCount = (int) (2 * Map.Height);

		var vertexCount =
			oddColumnVertexCount * oddColumnCount +
			evenColumnVertexCount * evenColumnCount +
			outsideEvenColumnRightVertexCount * outsideEvenColumnCount;

		var vertices = new Vector3[vertexCount];

		{
			int i = 0;

			for (var iOddColumn = 0; iOddColumn < oddColumnCount; iOddColumn++) {
				var col = iOddColumn * 2;

				vertices[i++] = GetVertex(new OffsetPosition(col, 0), FlatTopHexVertex.TopLeft);
				vertices[i++] = GetVertex(new OffsetPosition(col, 0), FlatTopHexVertex.TopRight);

				for (var row = 0; row < Map.Height; row++) {
					vertices[i++] = GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.Right);
					vertices[i++] = GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.BottomRight);
					vertices[i++] = GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.BottomLeft);
					vertices[i++] = GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.Left);
				}

				// Protruding vertices of next even column
				if (iOddColumn < oddColumnCount - 1 || outsideEvenColumnCount == 1) {
					vertices[i++] =
						GetVertex(new OffsetPosition(col + 1, (int)Map.Height - 1), FlatTopHexVertex.BottomLeft);
					vertices[i++] =
						GetVertex(new OffsetPosition(col + 1, (int)Map.Height - 1), FlatTopHexVertex.BottomRight);
				}
			}

			if (outsideEvenColumnCount == 1) {
				for (var row = 0; row < Map.Height; row++) {
					vertices[i++] = GetVertex(new OffsetPosition((int)Map.Width - 1, row), FlatTopHexVertex.TopRight);
					vertices[i++] = GetVertex(new OffsetPosition((int)Map.Width - 1, row), FlatTopHexVertex.Right);
				}
			}

			Assert.IsTrue(i == vertexCount);
		}


		// Indices

		var oddColumnEdgeCount = 1 + 5 * Map.Height;
		var outsideEvenColumnRightEdgeCount = 2 * Map.Height - 1;
		var evenColumnEdgeCount = 1 * Map.Height + 3;

		var edgeCount =
			oddColumnEdgeCount * oddColumnCount +
			outsideEvenColumnRightEdgeCount * outsideEvenColumnCount +
			evenColumnEdgeCount * evenColumnCount;

		var indices = new int[edgeCount * 2];

		int oddColumnVertexIndexStride = oddColumnVertexCount + 2;

		{
			int i = 0;

			for (var iOddColumn = 0; iOddColumn < oddColumnCount; iOddColumn++) {
				int columnVerticesBaseIndex = iOddColumn * oddColumnVertexIndexStride;

				indices[i++] = columnVerticesBaseIndex;
				indices[i++] = columnVerticesBaseIndex + 1;

				indices[i++] = columnVerticesBaseIndex + 1;
				indices[i++] = columnVerticesBaseIndex + 2;

				indices[i++] = columnVerticesBaseIndex + 2;
				indices[i++] = columnVerticesBaseIndex + 3;

				indices[i++] = columnVerticesBaseIndex + 3;
				indices[i++] = columnVerticesBaseIndex + 4;

				indices[i++] = columnVerticesBaseIndex + 4;
				indices[i++] = columnVerticesBaseIndex + 5;

				indices[i++] = columnVerticesBaseIndex + 5;
				indices[i++] = columnVerticesBaseIndex;

				for (var row = 1; row < Map.Height; row++) {
					int cellVerticesBaseIndex = columnVerticesBaseIndex + 2 + row * 4;

					indices[i++] = cellVerticesBaseIndex - 3;
					indices[i++] = cellVerticesBaseIndex;

					indices[i++] = cellVerticesBaseIndex;
					indices[i++] = cellVerticesBaseIndex + 1;

					indices[i++] = cellVerticesBaseIndex + 1;
					indices[i++] = cellVerticesBaseIndex + 2;

					indices[i++] = cellVerticesBaseIndex + 2;
					indices[i++] = cellVerticesBaseIndex + 3;

					indices[i++] = cellVerticesBaseIndex + 3;
					indices[i++] = cellVerticesBaseIndex - 2;
				}
			}

			for (var iEvenColumn = 0; iEvenColumn < insideEvenColumnCount; iEvenColumn++) {
				// Edges between odd columns
				for (var row = 0; row < Map.Height; row++) {
					int leftVertexIndex = oddColumnVertexIndexStride * iEvenColumn + (2 + row * 4);

					indices[i++] = leftVertexIndex;
					indices[i++] = leftVertexIndex + oddColumnVertexIndexStride + 3;
				}

				// Protruding part

				int protrudingVerticesBaseIndex = oddColumnVertexCount + oddColumnVertexIndexStride * iEvenColumn;

				// Left-bottom
				indices[i++] = protrudingVerticesBaseIndex - 3;
				indices[i++] = protrudingVerticesBaseIndex;

				// Bottom
				indices[i++] = protrudingVerticesBaseIndex;
				indices[i++] = protrudingVerticesBaseIndex + 1;

				// Right-bottom
				indices[i++] = protrudingVerticesBaseIndex + 1;
				indices[i++] = protrudingVerticesBaseIndex + oddColumnVertexCount;
			}

			if (outsideEvenColumnCount == 1) {
				// Right vertices
				int rightVerticesBaseIndex = oddColumnVertexIndexStride * oddColumnCount;
				for (var row = 0; row < Map.Height; row++) {
					int cellVerticesBaseIndex = rightVerticesBaseIndex + row * 2;

					// Top
					indices[i++] = cellVerticesBaseIndex - (4 + 4 * ((int)Map.Height - 1 - row) + 2 + 2 * row);
					indices[i++] = cellVerticesBaseIndex;

					// Right-top
					indices[i++] = cellVerticesBaseIndex;
					indices[i++] = cellVerticesBaseIndex + 1;

					// Right-bottom
					if (row < Map.Height - 1) {
						indices[i++] = cellVerticesBaseIndex + 1;
						indices[i++] = cellVerticesBaseIndex + 2;
					}
				}

				// Protruding part

				int protrudingVerticesBaseIndex =
					oddColumnVertexCount + oddColumnVertexIndexStride * (oddColumnCount - 1);

				// Left-bottom
				indices[i++] = protrudingVerticesBaseIndex - 3;
				indices[i++] = protrudingVerticesBaseIndex;

				// Bottom
				indices[i++] = protrudingVerticesBaseIndex;
				indices[i++] = protrudingVerticesBaseIndex + 1;

				// Right-bottom
				indices[i++] = protrudingVerticesBaseIndex + 1;
				indices[i++] = protrudingVerticesBaseIndex + 1 + outsideEvenColumnRightVertexCount;
			}

			Assert.IsTrue(i == edgeCount * 2);
		}


		var mesh = new Mesh();
		mesh.SetVertices(vertices);
		mesh.SetIndices(indices, MeshTopology.Lines, 0);

		return mesh;
	}
}



}
