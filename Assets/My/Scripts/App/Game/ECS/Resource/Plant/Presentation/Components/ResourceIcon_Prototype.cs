using Unity.Entities;



namespace App.Game.ECS.Resource.Plant.Presentation.Components {



public struct ResourceIcon_Prototype : IComponentData
{
	public Entity Entity;


	public ResourceIcon_Prototype(Entity entity)
	{
		Entity = entity;
	}
}



}
