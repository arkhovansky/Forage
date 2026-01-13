using UnityEngine;

using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Presentation {



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



public interface ITerrainType_GraphicalPresentation_Repository
{
	TerrainTypePresentation Get(TerrainTypeId typeId);
}



}
