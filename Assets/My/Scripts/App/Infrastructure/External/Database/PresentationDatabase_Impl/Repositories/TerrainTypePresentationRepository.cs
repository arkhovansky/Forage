using System.Collections.Generic;

using UnityEngine;

using Lib.VisualGrid;

using App.Application.PresentationDatabase;
using App.Game.Database;



namespace App.Infrastructure.External.Database.PresentationDatabase_Impl.Repositories {



public class TerrainTypePresentationRepository : ITerrainTypePresentationRepository
{
	private readonly HexLayout3D _hexLayout;

	private readonly Dictionary<TerrainTypeId, TerrainTypePresentation> _terrainTypes = new();


	//----------------------------------------------------------------------------------------------


	public TerrainTypePresentationRepository(HexLayout3D hexLayout)
	{
		_hexLayout = hexLayout;

		var flatMesh = CreateFlatTileMesh();

		var terrainTypesDB = GameDatabase.Instance.Presentation.TerrainTypes;

		_terrainTypes[TerrainTypeId.FreshWater] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.FreshWater).Material);
		_terrainTypes[TerrainTypeId.Sea] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.Sea).Material);
		_terrainTypes[TerrainTypeId.Ocean] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.Ocean).Material);
		_terrainTypes[TerrainTypeId.Grasslands] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.Grasslands).Material);
		_terrainTypes[TerrainTypeId.Plains] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.Plains).Material);
		_terrainTypes[TerrainTypeId.Forest] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.Forest).Material);
		_terrainTypes[TerrainTypeId.TropicalForest] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.TropicalForest).Material);
		_terrainTypes[TerrainTypeId.SwampyTropicalForest] = new TerrainTypePresentation(
			flatMesh,
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.SwampyTropicalForest).Material);
		_terrainTypes[TerrainTypeId.Hills] = new TerrainTypePresentation(
			CreateHillsMesh(),
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.Hills).Material);
		_terrainTypes[TerrainTypeId.Mountains] = new TerrainTypePresentation(
			CreateMountainsMesh(),
			terrainTypesDB.GetTerrainTypeData(TerrainTypeId.Mountains).Material);
	}


	public TerrainTypePresentation Get(TerrainTypeId terrainTypeId)
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
