using Unity.Entities;



namespace App.Game.ECS.Resource.Plant.Presentation.Components {



public struct ResourceIcon : IBufferElementData
{
	public Entity Entity;


	public ResourceIcon(Entity entity)
	{
		Entity = entity;
	}
}



}
