using UnityEngine;

using System.Collections.Generic;

using Lib.VisualGrid;



namespace App.Services.Terrain {



public class TerrainTypePresentationRepository : ITerrainTypePresentationRepository
{
	private readonly HexLayout3D _hexLayout;

	private readonly Dictionary<uint, TerrainTypePresentation> _terrainTypes = new();


	//----------------------------------------------------------------------------------------------


	public TerrainTypePresentationRepository(HexLayout3D hexLayout)
	{
		_hexLayout = hexLayout;

		var flatMesh = CreateFlatTileMesh();

		var freshWaterMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/FreshWater");
		var seaMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/Sea");
		var oceanMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/Ocean");
		var grasslandsMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/Grasslands");
		var plainsMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/Plains");
		var forestMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/Forest");
		var tropicalForestMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/TropicalForest");
		var swampyTropicalForestMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/SwampyTropicalForest");
		var hillsMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/Hills");
		var mountainsMaterial = UnityEngine.Resources.Load<Material>("Materials/Terrain/Mountains");


		_terrainTypes[0] = new TerrainTypePresentation(flatMesh, freshWaterMaterial);
		_terrainTypes[1] = new TerrainTypePresentation(flatMesh, seaMaterial);
		_terrainTypes[2] = new TerrainTypePresentation(flatMesh, oceanMaterial);
		_terrainTypes[3] = new TerrainTypePresentation(flatMesh, grasslandsMaterial);
		_terrainTypes[4] = new TerrainTypePresentation(flatMesh, plainsMaterial);
		_terrainTypes[5] = new TerrainTypePresentation(flatMesh, forestMaterial);
		_terrainTypes[6] = new TerrainTypePresentation(flatMesh, tropicalForestMaterial);
		_terrainTypes[7] = new TerrainTypePresentation(flatMesh, swampyTropicalForestMaterial);
		_terrainTypes[8] = new TerrainTypePresentation(CreateHillsMesh(), hillsMaterial);
		_terrainTypes[9] = new TerrainTypePresentation(CreateMountainsMesh(), mountainsMaterial);
	}


	public TerrainTypePresentation Get(uint terrainTypeId)
	{
		return _terrainTypes[terrainTypeId];
	}


	//----------------------------------------------------------------------------------------------
	// private

	private Mesh CreateFlatTileMesh()
	{
		return _hexLayout.GetCellMesh();
	}


	private Mesh CreateHillsMesh()
	{
		return CreateElevationsMesh(0.07f);
	}

	private Mesh CreateMountainsMesh()
	{
		return CreateElevationsMesh(0.2f);
	}


	private Mesh CreateElevationsMesh(float elevationHeight)
	{
		IReadOnlyList<Vector3> borderVertices = _hexLayout.GetCellBorderVertices();
		var center = Vector3.zero;

		var vertices = new Vector3[6 * 3 * 3];
		var triangles = new int[6 * 3 * 3];

		for (int fragment = 0; fragment < 6; ++fragment) {
			var borderVertex2Index = fragment < 5 ? fragment + 1 : 0;

			CreateElevation(new [] { borderVertices[fragment], borderVertices[borderVertex2Index], center },
			                elevationHeight,
			                vertices, triangles,
			                fragment);
		}

		var mesh = new Mesh { vertices = vertices, triangles = triangles };
		mesh.RecalculateNormals();

		return mesh;
	}


	private void CreateElevation(Vector3[] baseVertices, float height,
	                             Vector3[] meshVertices, int[] meshTriangles,
	                             int fragment)
	{
		var topX = (baseVertices[0].x + baseVertices[1].x + baseVertices[2].x) / 3;
		var topZ = (baseVertices[0].z + baseVertices[1].z + baseVertices[2].z) / 3;
		var top = new Vector3(topX, height, topZ);

		var verticesIndex = fragment * 3 * 3;
		var trianglesIndex = fragment * 3 * 3;

		for (uint face = 0; face < baseVertices.Length; ++face) {
			meshVertices[verticesIndex] = baseVertices[face];
			var index2 = face < baseVertices.Length - 1 ? face + 1 : 0;
			meshVertices[verticesIndex + 1] = baseVertices[index2];
			meshVertices[verticesIndex + 2] = top;

			meshTriangles[trianglesIndex++] = verticesIndex;
			meshTriangles[trianglesIndex++] = verticesIndex + 1;
			meshTriangles[trianglesIndex++] = verticesIndex + 2;

			verticesIndex += 3;
		}
	}
}



}
