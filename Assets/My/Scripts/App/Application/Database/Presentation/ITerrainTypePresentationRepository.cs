using UnityEngine;

using App.Game.Database;



namespace App.Application.Database.Presentation {



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
	string GetName(TerrainTypeId typeId);

	TerrainTypePresentation Get(TerrainTypeId typeId);
}



}
