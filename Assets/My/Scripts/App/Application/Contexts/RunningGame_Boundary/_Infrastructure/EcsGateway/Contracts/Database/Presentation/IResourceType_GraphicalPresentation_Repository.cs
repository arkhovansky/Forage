using UnityEngine;

using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Presentation {



public struct ResourceTypePresentation
{
	public readonly Mesh Mesh;
	public readonly Material Material;


	public ResourceTypePresentation(Mesh mesh, Material material)
	{
		Mesh = mesh;
		Material = material;
	}
}



public interface IResourceType_GraphicalPresentation_Repository
{
	ResourceTypePresentation Get(ResourceTypeId typeId);
}



}
