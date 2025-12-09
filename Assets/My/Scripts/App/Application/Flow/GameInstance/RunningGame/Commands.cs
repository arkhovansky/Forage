using Lib.AppFlow;
using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame {



public class EnterPlaceCampMode : ICommand {}



public class PlaceCamp : ICommand
{
	public AxialPosition Position { get; set; }


	public PlaceCamp(AxialPosition position)
	{
		Position = position;
	}
}



public class RunYearPeriod : ICommand
{
}



}
