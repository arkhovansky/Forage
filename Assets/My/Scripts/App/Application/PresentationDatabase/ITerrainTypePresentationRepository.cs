using UnityEngine;

using App.Game.Database;



namespace App.Application.PresentationDatabase {



public struct TerrainTypePresentation
{
	public readonly Mesh Mesh;
	public readonly Material Material;


	public TerrainTypePresentation(Mesh mesh, Material material)
	{
		Mesh = mesh;
		Material = material;
	}
}



public interface ITerrainTypePresentationRepository
{
	TerrainTypePresentation Get(TerrainTypeId terrainTypeId);
}



}
