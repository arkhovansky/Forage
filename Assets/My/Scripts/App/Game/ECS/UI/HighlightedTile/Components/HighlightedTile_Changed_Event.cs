using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.UI.HighlightedTile.Components {



public struct HighlightedTile_Changed_Event : IComponentData
{
	public AxialPosition? NewPosition;


	public HighlightedTile_Changed_Event(AxialPosition? newPosition)
	{
		NewPosition = newPosition;
	}
}



}
