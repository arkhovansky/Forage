using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

using Lib.AppFlow;
using Lib.Grid;
using Lib.VisualGrid;

using App.Application.Flow.GameInstance.RunningGame.Messages.InputEvents;
using App.Application.Flow.GameInstance.RunningGame.Messages.PresentationEvents;



namespace App.Infrastructure.External.UI.GameInstance.RunningGame {



/// <summary>
/// Acts as both View and Controller for the scene. Encapsulates low-level details of reading user input, moving camera,
/// and generates high-level hovering/selection events.
/// </summary>
public class SceneViewController : View
{
	private const int MapOverviewVerticalMargin = 50;
	private const float ZoomSpeed = 1.0f;


	private readonly Camera _camera;

	private readonly VisualRectangularHexMap3D _map;

	private readonly IInputEvent_Emitter _inputEvent_Emitter;

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


	public SceneViewController(Camera camera,
	                           VisualRectangularHexMap3D map,
	                           IInputEvent_Emitter inputEvent_Emitter)
	{
		_camera = camera;
		_map = map;
		_inputEvent_Emitter = inputEvent_Emitter;

		_pointAction = InputSystem.actions.FindAction("Point");
		_clickAction = InputSystem.actions.FindAction("Click");
		_rightClickAction = InputSystem.actions.FindAction("RightClick");
		_zoomAction = InputSystem.actions.FindAction("ScrollWheel");

		base.Add_PresentationEvent_Handler<PositionCameraToOverview_Request>(On_PositionCameraToOverview);
	}


	//----------------------------------------------------------------------------------------------
	// ILoopComponent implementation overrides


	public override void Update()
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
				_inputEvent_Emitter.Emit(new TileClicked(_hoveredTile.Value));
		}
	}


	//----------------------------------------------------------------------------------------------
	// Message handlers


	private void On_PositionCameraToOverview(PositionCameraToOverview_Request _)
	{
		PositionCameraToOverview();
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void PositionCameraToOverview()
	{
		var mapScreenRect = new RectInt(0, MapOverviewVerticalMargin,
		                                _camera.pixelWidth, _camera.pixelHeight - 2 * MapOverviewVerticalMargin);
		FitMapRectToScreenRect(_map.BoundingRect2D, mapScreenRect);
	}


	/// <summary>
	/// Set camera position so that rectangle in map plane fits into screen-space rectangle.
	/// </summary>
	/// <param name="mapRect">Rectangle in map plane.</param>
	/// <param name="screenRect">Rectangle in screen space.</param>
	/// <remarks>
	/// Assumes that camera is tilted down around X to angle (0, 90].
	/// [TODO] Fits only vertically currently.
	/// [TODO] Not pixel-perfect.
	/// </remarks>
	private void FitMapRectToScreenRect(Rect mapRect, RectInt screenRect)
	{
		Assert.IsTrue(_camera.transform.eulerAngles.x is > 0 and <= 90 &&
		              _camera.transform.eulerAngles is {y: 0, z: 0});

		// Consider projection to YZ plane.
		// Border rays are rays from camera through top and bottom points (as on screen) of map rect.
		// Consider camera frustum section named "d1s", distance to which = 1.
		// Indices 1 and 2 mean lower or upper (as on screen) relative to frustum normal.

		float halfD1SHeight = Mathf.Tan(_camera.fieldOfView / 2f * Mathf.Deg2Rad);
		float halfPixelHeight = _camera.pixelHeight / 2f;

		// Segments on d1s between border rays and frustum normal
		float l1_d1s = halfD1SHeight / halfPixelHeight * (screenRect.yMax - halfPixelHeight);
		float l2_d1s = halfD1SHeight / halfPixelHeight * (halfPixelHeight - screenRect.yMin);

		// Angle between frustum normal and map normal
		float normalsAngle = (90 - _camera.transform.eulerAngles.x) * Mathf.Deg2Rad;

		// Angles between border rays and map normal
		float angle1 = Mathf.Atan(l1_d1s) - normalsAngle;
		float angle2 = Mathf.Atan(l2_d1s) + normalsAngle;

		float tan_angle1 = Mathf.Tan(angle1);

		float cameraY = mapRect.height / (tan_angle1 + Mathf.Tan(angle2));

		// Segment on the map between lower border ray and map normal
		float l1_map = cameraY * tan_angle1;

		float cameraZ = (mapRect.y - mapRect.height) + l1_map;

		_camera.transform.position = new Vector3(mapRect.center.x, cameraY, cameraZ);
	}


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
		_inputEvent_Emitter.Emit(new HoveredTileChanged(newTile));
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
