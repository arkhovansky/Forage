using Lib.Grid;

using App.Application.Framework.UICore.Flow;



namespace App.Application.Flow.GameInstance.RunningGame {



public record HoveredTileChanged(
	AxialPosition? Position
) : ICommand;



public record TileClicked(
	AxialPosition Position
) : ICommand;



public class YearPeriodChanged : ICommand {}



}
