using Unity.Entities;
using UnityEngine.InputSystem;

using Lib.Grid;

using App.Client.Framework.UICore.HighLevel;
using App.Game.Ecs.Components.Singletons.HoveredTile;
using App.Game.Ecs.Components.Singletons.YearPeriod;



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
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entityQuery = entityManager.CreateEntityQuery(typeof(CurrentYearPeriod));
		var singletonEntity = entityQuery.GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, new HoveredTileChanged_Event(tilePosition));
	}
}



}
