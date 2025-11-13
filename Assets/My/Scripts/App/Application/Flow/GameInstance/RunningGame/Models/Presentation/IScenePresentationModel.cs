using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Presentation {



public interface IScenePresentationModel : IScenePresentationModel_RO
{
	new AxialPosition? HoveredTile { get; set; }
}



}
