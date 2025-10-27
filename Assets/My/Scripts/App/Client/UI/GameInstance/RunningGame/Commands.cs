using Lib.Grid;

using App.Client.Framework.UICore.HighLevel;



namespace App.Client.UI.GameInstance.RunningGame {



public class EnterPlaceCampMode : ICommand {}



public class PlaceCamp : ICommand
{
	public AxialPosition Position { get; set; }


	public PlaceCamp(AxialPosition position)
	{
		Position = position;
	}
}



public class EndTurnCommand : ICommand
{
}



}
