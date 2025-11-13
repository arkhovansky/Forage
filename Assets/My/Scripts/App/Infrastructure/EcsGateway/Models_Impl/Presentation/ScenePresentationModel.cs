using Lib.Grid;

using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models_Impl.Presentation {



public class ScenePresentationModel : IScenePresentationModel
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
}



}
