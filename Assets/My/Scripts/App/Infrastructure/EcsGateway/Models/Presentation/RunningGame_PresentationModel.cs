using Lib.Grid;

using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models.Presentation {



public class RunningGame_PresentationModel : IRunningGame_PresentationModel
{
	private AxialPosition? _hoveredTile;


	public AxialPosition? HoveredTile {
		get => _hoveredTile;

		set {
			if (_hoveredTile == value)
				return;

			EcsService.SendEcsCommand(new HoveredTileChanged_Event(value));
			_hoveredTile = value;
		}
	}


	public bool Is_CampPlacing_Mode { get; set; } = false;
}



}
