using UnityEngine;



namespace App.Services.Resources {



public struct ResourceTypePresentation
{
	public Mesh Mesh;
	public Material Material;
}



public interface IResourceTypePresentationRepository
{
	ResourceTypePresentation Get(uint resourceTypeId);
}



}
