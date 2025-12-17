using Unity.Entities;



namespace App.Game.ECS.UI.HighlightedTile.Components {



public struct HighlightedTileEntity : IComponentData
{
	public Entity Entity;


	public HighlightedTileEntity(Entity entity)
	{
		Entity = entity;
	}
}



}
