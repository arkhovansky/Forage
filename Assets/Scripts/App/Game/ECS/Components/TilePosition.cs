using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.Components {



public struct TilePosition : IComponentData
{
	public AxialPosition Position;



	public TilePosition(AxialPosition position)
	{
		Position = position;
	}
}



}
