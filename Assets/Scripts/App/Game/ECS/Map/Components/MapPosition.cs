using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.Map.Components {



public struct MapPosition : IComponentData
{
	public AxialPosition Position;



	public MapPosition(AxialPosition position)
	{
		Position = position;
	}
}



}
