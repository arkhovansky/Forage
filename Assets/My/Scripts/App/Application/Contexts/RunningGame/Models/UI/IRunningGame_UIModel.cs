using Lib.Grid;



namespace App.Application.Contexts.RunningGame.Models.UI {



public interface IRunningGame_UIModel : IRunningGame_UIModel_RO
{
	new AxialPosition? HighlightedTile { get; set; }

	new bool Is_CampPlacing_Mode { get; set; }
}



}
