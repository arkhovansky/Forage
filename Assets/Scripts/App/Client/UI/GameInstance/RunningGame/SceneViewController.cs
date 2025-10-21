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
	private const float ZoomSpeed = 1.0f;


	private readonly Camera _camera;

	private readonly VisualRectangularHexMap3D _map;

	private readonly InputAction _pointAction;
	private readonly InputAction _clickAction;
	private readonly InputAction _rightClickAction;
	private readonly InputAction _zoomAction;

	private AxialPosition? _hoveredTile;


	private record MapScrollMode(
		Vector3 Anchor,
		Plane AnchorPlane,
		Camera Camera
	) {
		public void Update(Vector2 screenPoint)
		{
			var cameraRay = Camera.ScreenPointToRay(new Vector3(screenPoint.x, screenPoint.y, 0));
			if (!AnchorPlane.Raycast(cameraRay, out var distance))
				return;
			var worldPoint = cameraRay.GetPoint(distance);

			Camera.transform.Translate(Anchor - worldPoint, Space.World);
		}
	}

	private MapScrollMode? _mapScrollMode;


	//----------------------------------------------------------------------------------------------


	public SceneViewController(Camera camera, VisualRectangularHexMap3D map,
	                           ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_camera = camera;
		_map = map;

		_pointAction = InputSystem.actions.FindAction("Point");
		_clickAction = InputSystem.actions.FindAction("Click");
		_rightClickAction = InputSystem.actions.FindAction("RightClick");
		_zoomAction = InputSystem.actions.FindAction("ScrollWheel");
	}


	protected override void DoUpdate()
	{
		var screenPoint = _pointAction.ReadValue<Vector2>();

		if (_rightClickAction.WasReleasedThisFrame())
			_mapScrollMode = null;

		if (_mapScrollMode != null)
			_mapScrollMode.Update(screenPoint);
		else {
			if (_rightClickAction.IsPressed()) {
				if (GetMapPlanePoint(screenPoint, out Vector3 mapPoint))
					_mapScrollMode = new MapScrollMode(mapPoint, _map.Layout.Plane, _camera);
			}
		}

		var zoomControlDelta = _zoomAction.ReadValue<Vector2>().y;
		Zoom(zoomControlDelta);

		UpdateHoveredTile(screenPoint);

		if (_clickAction.WasPerformedThisFrame()) {
			if (_hoveredTile.HasValue)
				EmitCommand(new TileClicked(_hoveredTile.Value));
		}
	}


	//----------------------------------------------------------------------------------------------
	// private


	private bool GetMapPlanePoint(Vector2 screenPoint, out Vector3 mapPoint)
	{
		return _map.Layout.GetPoint(GetRay(screenPoint), out mapPoint);
	}


	private void Zoom(float zoomControlValue)
	{
		_camera.transform.Translate(0, 0, zoomControlValue * ZoomSpeed);
	}


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
