using Lib.Grid;

using Unity.Entities;



namespace App.Game.Ecs.Components.Singletons.HoveredTile {



public struct HoveredTileChanged_Event : IComponentData
{
	public AxialPosition? NewPosition;


	public HoveredTileChanged_Event(AxialPosition? newPosition)
	{
		NewPosition = newPosition;
	}
}



}
