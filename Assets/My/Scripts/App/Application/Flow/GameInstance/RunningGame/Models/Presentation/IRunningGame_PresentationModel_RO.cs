using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Presentation {



public interface IRunningGame_PresentationModel_RO
{
	AxialPosition? HighlightedTile { get; }

	bool Is_CampPlacing_Mode { get; }
}



}
