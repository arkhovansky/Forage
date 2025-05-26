using Lib.Grid;

using App.Game.ECS.UI.HoveredTile.Components;
using App.Services;



namespace App.Client.UI.GameInstance.RunningGame {



public class DefaultUIMode : IUIMode
{
	public void Update(AxialPosition? oldHoveredTilePosition, AxialPosition? newHoveredTilePosition)
	{
		if (newHoveredTilePosition != oldHoveredTilePosition)
			NotifySystems_HoveredTileChanged(newHoveredTilePosition);
	}


	private void NotifySystems_HoveredTileChanged(AxialPosition? tilePosition)
	{
		EcsService.SendEcsCommand(new HoveredTileChanged_Event(tilePosition));
	}
}



}
