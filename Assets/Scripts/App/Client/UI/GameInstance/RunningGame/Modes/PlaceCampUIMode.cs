using UnityEngine.InputSystem;

using Lib.Grid;

using App.Client.Framework.UICore.HighLevel;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Services;



namespace App.Client.UI.GameInstance.RunningGame {



public class PlaceCampUIMode : IUIMode
{
	private readonly ICommandRouter _commandRouter;
	private readonly IController _controller;

	private readonly InputAction _clickAction;



	public PlaceCampUIMode(ICommandRouter commandRouter, IController controller)
	{
		_commandRouter = commandRouter;
		_controller = controller;

		_clickAction = InputSystem.actions.FindAction("Click");
	}


	public void Update(AxialPosition? oldHoveredTilePosition, AxialPosition? newHoveredTilePosition)
	{
		if (newHoveredTilePosition != oldHoveredTilePosition)
			NotifySystems_HoveredTileChanged(newHoveredTilePosition);

		if (_clickAction.WasPerformedThisFrame() && newHoveredTilePosition.HasValue) {
			_commandRouter.EmitCommand(new PlaceCamp(newHoveredTilePosition.Value), _controller);
		}
	}


	private void NotifySystems_HoveredTileChanged(AxialPosition? tilePosition)
	{
		EcsService.SendEcsCommand(new HoveredTileChanged_Event(tilePosition));
	}
}



}
