using Unity.Entities;

using Lib.Grid;



namespace App.Game.Ecs.Components {



public struct TilePosition : IComponentData
{
	public AxialPosition Position;



	public TilePosition(AxialPosition position)
	{
		Position = position;
	}
}



}
