using Unity.Entities;



namespace App.Game.Ecs.Components.Singletons.HoveredTile {



public struct HoveredTileEntity : IComponentData
{
	public Entity Entity;


	public HoveredTileEntity(Entity entity)
	{
		Entity = entity;
	}
}



}
