using UnityEngine;

using App.Game.Database;



namespace App.Application.PresentationDatabase {



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



public interface IResourceTypePresentationRepository
{
	ResourceTypePresentation Get(ResourceTypeId resourceTypeId);
}



}
