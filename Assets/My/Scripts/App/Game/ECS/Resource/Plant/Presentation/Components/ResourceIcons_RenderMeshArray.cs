using Unity.Entities;
using Unity.Rendering;



namespace App.Game.ECS.Resource.Plant.Presentation.Components {



public class ResourceIcons_RenderMeshArray : IComponentData
{
	public RenderMeshArray Value;


	public static implicit operator RenderMeshArray(ResourceIcons_RenderMeshArray x)
		=> x.Value;
}



}
