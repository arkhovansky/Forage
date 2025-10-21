using UnityEngine;
using UnityEngine.InputSystem;

using Lib.Grid;
using Lib.VisualGrid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.HighLevel.Impl;



namespace App.Client.UI.GameInstance.RunningGame {



/// <summary>
/// Acts as both View and Controller for the scene. Encapsulates low-level details of reading user input, moving camera,
/// and generates high-level hovering/selection events.
/// </summary>
/// <remarks>
/// It should be a child of <see cref="RunningGameController"/> with identical lifetime.
/// </remarks>
public class SceneViewController : Controller
{
	private readonly Camera _camera;

	private readonly VisualRectangularHexMap3D _map;

	private readonly InputAction _pointAction;
	private readonly InputAction _clickAction;

	private AxialPosition? _hoveredTile;


	//----------------------------------------------------------------------------------------------


	public SceneViewController(Camera camera, VisualRectangularHexMap3D map,
	                           ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_camera = camera;
		_map = map;

		_pointAction = InputSystem.actions.FindAction("Point");
		_clickAction = InputSystem.actions.FindAction("Click");
	}


	protected override void DoUpdate()
	{
		var screenPoint = _pointAction.ReadValue<Vector2>();

		UpdateHoveredTile(screenPoint);

		if (_clickAction.WasPerformedThisFrame()) {
			if (_hoveredTile.HasValue)
				EmitCommand(new TileClicked(_hoveredTile.Value));
		}
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void UpdateHoveredTile(Vector2 screenPoint)
	{
		var newTile = GetHoveredTile(screenPoint);
		if (newTile == _hoveredTile)
			return;
		_hoveredTile = newTile;
		EmitCommand(new HoveredTileChanged(newTile));
	}


	private AxialPosition? GetHoveredTile(Vector2 screenPoint)
	{
		return _map.GetAxialPosition(GetRay(screenPoint));
	}


	private Ray GetRay(Vector2 screenPoint)
	{
		return _camera.ScreenPointToRay(new Vector3(screenPoint.x, screenPoint.y, 0));
	}
}



}
