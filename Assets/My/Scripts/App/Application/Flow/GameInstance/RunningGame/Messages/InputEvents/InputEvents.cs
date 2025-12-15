using Lib.AppFlow;
using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Messages.InputEvents {



public record HoveredTileChanged(
	AxialPosition? Position
) : IInputEvent;



public record TileClicked(
	AxialPosition Position
) : IInputEvent;



}
