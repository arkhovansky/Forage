using Lib.AppFlow;
using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame {



public record HoveredTileChanged(
	AxialPosition? Position
) : ICommand;



public record TileClicked(
	AxialPosition Position
) : ICommand;



public class YearPeriodChanged : ICommand {}



}
