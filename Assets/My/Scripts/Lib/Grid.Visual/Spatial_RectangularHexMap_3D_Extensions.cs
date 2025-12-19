using UnityEngine;
using UnityEngine.Assertions;

using Lib.Grid.Spatial;



namespace Lib.Grid.Visual {



public static class Spatial_RectangularHexMap_3D_Extensions
{
	public static Mesh GetGridLinesMesh(this Spatial_RectangularHexMap_3D self)
	{
		var map = self.Map;

		int oddColumnCount = (int) (map.Width / 2 + map.Width % 2);
		int evenColumnCount = (int) (map.Width - oddColumnCount);
		int outsideEvenColumnCount = (int) (1 - map.Width % 2);
		int insideEvenColumnCount = evenColumnCount - outsideEvenColumnCount;


		// Vertices

		int oddColumnVertexCount = (int) (2 + 4 * map.Height);
		const int evenColumnVertexCount = 2;
		int outsideEvenColumnRightVertexCount = (int) (2 * map.Height);

		var vertexCount =
			oddColumnVertexCount * oddColumnCount +
			evenColumnVertexCount * evenColumnCount +
			outsideEvenColumnRightVertexCount * outsideEvenColumnCount;

		var vertices = new Vector3[vertexCount];

		{
			int i = 0;

			for (var iOddColumn = 0; iOddColumn < oddColumnCount; iOddColumn++) {
				var col = iOddColumn * 2;

				vertices[i++] = self.GetVertex(new OffsetPosition(col, 0), FlatTopHexVertex.TopLeft);
				vertices[i++] = self.GetVertex(new OffsetPosition(col, 0), FlatTopHexVertex.TopRight);

				for (var row = 0; row < map.Height; row++) {
					vertices[i++] = self.GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.Right);
					vertices[i++] = self.GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.BottomRight);
					vertices[i++] = self.GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.BottomLeft);
					vertices[i++] = self.GetVertex(new OffsetPosition(col, row), FlatTopHexVertex.Left);
				}

				// Protruding vertices of next even column
				if (iOddColumn < oddColumnCount - 1 || outsideEvenColumnCount == 1) {
					vertices[i++] = self.GetVertex(new OffsetPosition(col + 1, (int)map.Height - 1),
					                               FlatTopHexVertex.BottomLeft);
					vertices[i++] = self.GetVertex(new OffsetPosition(col + 1, (int)map.Height - 1),
					                               FlatTopHexVertex.BottomRight);
				}
			}

			if (outsideEvenColumnCount == 1) {
				for (var row = 0; row < map.Height; row++) {
					vertices[i++] = self.GetVertex(new OffsetPosition((int)map.Width - 1, row),
					                               FlatTopHexVertex.TopRight);
					vertices[i++] = self.GetVertex(new OffsetPosition((int)map.Width - 1, row),
					                               FlatTopHexVertex.Right);
				}
			}

			Assert.IsTrue(i == vertexCount);
		}


		// Indices

		var oddColumnEdgeCount = 1 + 5 * map.Height;
		var outsideEvenColumnRightEdgeCount = 2 * map.Height - 1;
		var evenColumnEdgeCount = 1 * map.Height + 3;

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

				for (var row = 1; row < map.Height; row++) {
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
				for (var row = 0; row < map.Height; row++) {
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
				for (var row = 0; row < map.Height; row++) {
					int cellVerticesBaseIndex = rightVerticesBaseIndex + row * 2;

					// Top
					indices[i++] = cellVerticesBaseIndex - (4 + 4 * ((int)map.Height - 1 - row) + 2 + 2 * row);
					indices[i++] = cellVerticesBaseIndex;

					// Right-top
					indices[i++] = cellVerticesBaseIndex;
					indices[i++] = cellVerticesBaseIndex + 1;

					// Right-bottom
					if (row < map.Height - 1) {
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
