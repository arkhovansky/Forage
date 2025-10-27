using Unity.Entities;



namespace App.Game.ECS.Resource.Plant.Components {



public struct TilePlantResource : IComponentData
{
	public Entity ResourceEntity;


	public TilePlantResource(Entity resourceEntity)
	{
		ResourceEntity = resourceEntity;
	}
}



}
