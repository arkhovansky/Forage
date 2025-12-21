using Lib.AppFlow;
using Lib.Grid;



namespace App.Application.Contexts.RunningGame.Messages.InputEvents {



public record HoveredTileChanged(
	AxialPosition? Position
) : IInputEvent;



public record TileClicked(
	AxialPosition Position
) : IInputEvent;



}
