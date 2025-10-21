using Lib.Grid;

using App.Client.Framework.UICore.HighLevel;



namespace App.Client.UI.GameInstance.RunningGame {



public record HoveredTileChanged(
	AxialPosition? Position
) : ICommand;



public record TileClicked(
	AxialPosition Position
) : ICommand;



}
