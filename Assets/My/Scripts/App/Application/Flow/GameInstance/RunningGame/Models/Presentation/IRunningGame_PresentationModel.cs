using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Presentation {



public interface IRunningGame_PresentationModel : IRunningGame_PresentationModel_RO
{
	new AxialPosition? HoveredTile { get; set; }

	new bool Is_CampPlacing_Mode { get; set; }
}



}
