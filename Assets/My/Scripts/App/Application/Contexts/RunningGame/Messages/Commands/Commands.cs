using Lib.AppFlow;
using Lib.Grid;



namespace App.Application.Contexts.RunningGame.Messages.Commands {



public class EnterPlaceCampMode : ICommand {}



public record PlaceCamp(
	AxialPosition Position
) : ICommand;



public class RunYearPeriod : ICommand {}



}
