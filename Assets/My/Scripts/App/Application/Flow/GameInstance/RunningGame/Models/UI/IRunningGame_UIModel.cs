using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Models.UI {



public interface IRunningGame_UIModel : IRunningGame_UIModel_RO
{
	new AxialPosition? HighlightedTile { get; set; }

	new bool Is_CampPlacing_Mode { get; set; }
}



}
