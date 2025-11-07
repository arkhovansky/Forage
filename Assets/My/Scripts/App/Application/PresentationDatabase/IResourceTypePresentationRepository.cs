using UnityEngine;



namespace App.Application.PresentationDatabase {



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
