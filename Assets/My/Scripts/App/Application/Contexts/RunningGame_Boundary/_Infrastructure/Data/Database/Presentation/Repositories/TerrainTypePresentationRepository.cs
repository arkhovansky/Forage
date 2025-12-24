using System.Collections.Generic;

using UnityEngine;

using Lib.Grid.Spatial;
using Lib.Grid.Visual;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Game.Database;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class TerrainTypePresentationRepository : ITerrainTypePresentationRepository
{
	private readonly HexGridLayout_3D _gridLayout;


	private record TerrainType_Data(
		string Name,
		TerrainTypePresentation GraphicalData);

	private readonly Dictionary<TerrainTypeId, TerrainType_Data> _terrainTypes = new();


	//----------------------------------------------------------------------------------------------


	public TerrainTypePresentationRepository(TerrainTypes_Presentation asset, HexGridLayout_3D gridLayout)
	{
		_gridLayout = gridLayout;

		var flatMesh = CreateFlatTileMesh();

		var dbData = asset.GetTerrainTypeData(TerrainTypeId.FreshWater);
		_terrainTypes[TerrainTypeId.FreshWater] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.Sea);
		_terrainTypes[TerrainTypeId.Sea] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.Ocean);
		_terrainTypes[TerrainTypeId.Ocean] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.Grasslands);
		_terrainTypes[TerrainTypeId.Grasslands] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.Plains);
		_terrainTypes[TerrainTypeId.Plains] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.Forest);
		_terrainTypes[TerrainTypeId.Forest] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.TropicalForest);
		_terrainTypes[TerrainTypeId.TropicalForest] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.SwampyTropicalForest);
		_terrainTypes[TerrainTypeId.SwampyTropicalForest] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(flatMesh, dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.Hills);
		_terrainTypes[TerrainTypeId.Hills] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(CreateHillsMesh(), dbData.Material));

		dbData = asset.GetTerrainTypeData(TerrainTypeId.Mountains);
		_terrainTypes[TerrainTypeId.Mountains] = new TerrainType_Data(
			dbData.Name,
			new TerrainTypePresentation(CreateMountainsMesh(), dbData.Material));
	}


	public string GetName(TerrainTypeId typeId)
	{
		return _terrainTypes[typeId].Name;
	}


	public TerrainTypePresentation Get(TerrainTypeId typeId)
	{
		return _terrainTypes[typeId].GraphicalData;
	}


	//----------------------------------------------------------------------------------------------
	// private

	private Mesh CreateFlatTileMesh()
	{
		return _gridLayout.GetCellMesh();
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
		IReadOnlyList<Vector3> borderVertices = _gridLayout.GetCellBorderVertices();
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
