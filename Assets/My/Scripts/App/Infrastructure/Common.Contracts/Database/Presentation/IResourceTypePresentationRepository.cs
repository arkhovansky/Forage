using UnityEngine;

using App.Game.Database;



namespace App.Infrastructure.Common.Contracts.Database.Presentation {



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
	string GetName(ResourceTypeId typeId);

	ResourceTypePresentation Get(ResourceTypeId typeId);
}



}
