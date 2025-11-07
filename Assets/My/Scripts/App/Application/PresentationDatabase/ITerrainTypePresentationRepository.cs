using UnityEngine;



namespace App.Application.PresentationDatabase {



public class TerrainTypePresentation
{
	public Mesh Mesh;
	public Material Material;


	public TerrainTypePresentation(Mesh mesh, Material material)
	{
		Mesh = mesh;
		Material = material;
	}
}



public interface ITerrainTypePresentationRepository
{
	TerrainTypePresentation Get(uint terrainTypeId);
}



}
