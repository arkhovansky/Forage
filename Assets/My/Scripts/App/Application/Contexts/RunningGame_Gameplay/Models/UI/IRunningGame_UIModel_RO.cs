using Lib.Grid;



namespace App.Application.Contexts.RunningGame_Gameplay.Models.UI {



public interface IRunningGame_UIModel_RO
{
	AxialPosition? HighlightedTile { get; }

	bool Is_CampPlacing_Mode { get; }
}



}
