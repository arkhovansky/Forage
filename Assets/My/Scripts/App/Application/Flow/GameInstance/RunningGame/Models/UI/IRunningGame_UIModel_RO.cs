using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Models.UI {



public interface IRunningGame_UIModel_RO
{
	AxialPosition? HighlightedTile { get; }

	bool Is_CampPlacing_Mode { get; }
}



}
