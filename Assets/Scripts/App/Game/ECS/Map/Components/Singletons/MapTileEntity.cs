using Unity.Entities;



namespace App.Game.ECS.Map.Components.Singletons {



[InternalBufferCapacity(0)]
public struct MapTileEntity : IBufferElementData
{
	public Entity Value;



	public MapTileEntity(Entity value)
	{
		Value = value;
	}

	public static implicit operator Entity(MapTileEntity mapTileEntity)
		=> mapTileEntity.Value;
}



}
