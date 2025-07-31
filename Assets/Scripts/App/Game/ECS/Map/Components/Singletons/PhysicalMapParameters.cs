using Unity.Entities;



namespace App.Game.ECS.Map.Components.Singletons {



public readonly struct PhysicalMapParameters : IComponentData
{
	/// <summary>
	/// Inner diameter of a tile in km.
	/// </summary>
	public readonly float TileInnerDiameter;



	public PhysicalMapParameters(float tileInnerDiameter)
	{
		TileInnerDiameter = tileInnerDiameter;
	}
}



}
