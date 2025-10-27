using Unity.Entities;



namespace App.Game.ECS.UI.HoveredTile.Components {



public struct HoveredTileEntity : IComponentData
{
	public Entity Entity;


	public HoveredTileEntity(Entity entity)
	{
		Entity = entity;
	}
}



}
