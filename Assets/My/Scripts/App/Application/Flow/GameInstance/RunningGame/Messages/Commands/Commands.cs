using Lib.AppFlow;
using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Messages.Commands {



public class EnterPlaceCampMode : ICommand {}



public record PlaceCamp(
	AxialPosition Position
) : ICommand;



public class RunYearPeriod : ICommand {}



public class YearPeriodChanged : ICommand {}



}
