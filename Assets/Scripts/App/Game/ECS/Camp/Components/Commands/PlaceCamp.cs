using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.Camp.Components.Commands {



public struct PlaceCamp : IComponentData
{
	public AxialPosition Position;


	public PlaceCamp(AxialPosition position)
	{
		Position = position;
	}
}



}
