using Unity.Entities;
using Unity.Rendering;



namespace App.Game.ECS.Resource.Plant.Presentation.Components {



/// <summary>
/// Sparse array of MaterialMeshInfo indexed by resource type id.
/// </summary>
[InternalBufferCapacity(0)]
public struct ResourceIcon_MaterialMeshInfo : IBufferElementData
{
	public MaterialMeshInfo Value;


	public ResourceIcon_MaterialMeshInfo(MaterialMeshInfo value)
	{
		Value = value;
	}

	public static implicit operator MaterialMeshInfo(ResourceIcon_MaterialMeshInfo x)
		=> x.Value;
}



}
