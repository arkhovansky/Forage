using Unity.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

using UniMob;
using AtomLifetime = UniMob.Lifetime;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.View.Impl {



public class ResourceMarker_View
	: IResourceMarker_View,
	  IResourceMarker_View_ForPool
{
	private static readonly DataBinding Left_Binding;
	private static readonly DataBinding Bottom_Binding;
	private static readonly DataBinding Core_Width_Binding;
	private static readonly DataBinding CoreBorder_Width_Binding;
	private static readonly DataBinding BodyBorder_Width_Binding;


	static ResourceMarker_View()
	{
		Left_Binding = new DataBinding {
			dataSourcePath = PropertyPath.FromName(nameof(Left)),
			bindingMode = BindingMode.ToTarget
		};

		Bottom_Binding = new DataBinding {
			dataSourcePath = PropertyPath.FromName(nameof(Bottom)),
			bindingMode = BindingMode.ToTarget
		};

		Core_Width_Binding = new DataBinding {
			dataSourcePath = PropertyPath.FromName(nameof(Core_Width)),
			bindingMode = BindingMode.ToTarget
		};

		CoreBorder_Width_Binding = new DataBinding {
			dataSourcePath = PropertyPath.FromName(nameof(CoreBorder_Width)),
			bindingMode = BindingMode.ToTarget
		};

		BodyBorder_Width_Binding = new DataBinding {
			dataSourcePath = PropertyPath.FromName(nameof(BodyBorder_Width)),
			bindingMode = BindingMode.ToTarget
		};
	}


	//----------------------------------------------------------------------------------------------

	[CreateProperty]
	public float Left { get; private set; }

	[CreateProperty]
	public float Bottom { get; private set; }

	[CreateProperty]
	public float Core_Width { get; private set; }

	[CreateProperty]
	public float CoreBorder_Width { get; private set; }

	[CreateProperty]
	public float BodyBorder_Width { get; private set; }

	//----------------------------------------------------------------------------------------------

	private readonly IResourceMarker_RenderModel _model;

	private readonly IResourceType_Icon_Repository _resourceType_Icon_Repository;

	private readonly Camera _camera;

	/// <summary>
	/// Position of the anchor point (bottom center) in panel coordinates.
	/// </summary>
	private readonly Atom<Vector2> _panelPosition_Atom;

	//----------------------------------------------------------------------------------------------


	public ResourceMarker_View(
		IResourceMarker_RenderModel model,
		UIDocument uiDocument,
		IResourceType_Icon_Repository resourceType_Icon_Repository,
		Camera camera,
		Atom<Transform> cameraTransform_Atom,
		AtomLifetime atomLifetime)
	{
		_model = model;
		_resourceType_Icon_Repository = resourceType_Icon_Repository;
		UIDocument = uiDocument;
		_camera = camera;

		_panelPosition_Atom =
			Atom.Computed(atomLifetime, () =>
				              Compute_PanelPosition(cameraTransform_Atom.Value),
			              false, "ResourceMarker_View: panelPosition");

		Atom.Reaction(atomLifetime, () =>
			              Left = Compute_Left(_model.CoreRadius_Atom.Value, _model.BorderWidth_Atom.Value,
			                                  _panelPosition_Atom.Value),
		              null!, "ResourceMarker_View: Left");
		Atom.Reaction(atomLifetime, () =>
			              Bottom = Compute_Bottom(_panelPosition_Atom.Value),
		              null!, "ResourceMarker_View: Bottom");
		Atom.Reaction(atomLifetime, () =>
			              Core_Width = _model.CoreRadius_Atom.Value * 2,
		              null!, "ResourceMarker_View: Core_Width");
		Atom.Reaction(atomLifetime, () =>
			              CoreBorder_Width = _model.UnavailableRingWidth_Atom.Value,
		              null!, "ResourceMarker_View: CoreBorder_Width");
		Atom.Reaction(atomLifetime, () =>
			              BodyBorder_Width = _model.BorderWidth_Atom.Value,
		              null!, "ResourceMarker_View: BodyBorder_Width");

		BindData();
	}


	//----------------------------------------------------------------------------------------------
	// IResettable


	public void Reset()
	{
		UIDocument.rootVisualElement.Q("core").style.backgroundImage =
			_resourceType_Icon_Repository.Get(_model.ResourceType);

		UIDocument.sortingOrder = _model.Order;

		using (Atom.NoWatch)
			_panelPosition_Atom.Invalidate();
	}


	//----------------------------------------------------------------------------------------------
	// IResourceMarker_View_ForPool


	public UIDocument UIDocument { get; }


	//----------------------------------------------------------------------------------------------
	// private


	private void BindData()
	{
		var container = UIDocument.rootVisualElement.Q("container");
		var body = container.Q("body");
		var core = body.Q("core");

		container.dataSource = this;

		container.SetBinding("style.left", Left_Binding);
		container.SetBinding("style.bottom", Bottom_Binding);

		core.SetBinding("style.width", Core_Width_Binding);
		core.SetBinding("style.height", Core_Width_Binding);

		core.SetBinding("style.borderTopWidth", CoreBorder_Width_Binding);
		core.SetBinding("style.borderLeftWidth", CoreBorder_Width_Binding);
		core.SetBinding("style.borderRightWidth", CoreBorder_Width_Binding);
		core.SetBinding("style.borderBottomWidth", CoreBorder_Width_Binding);

		body.SetBinding("style.borderTopWidth", BodyBorder_Width_Binding);
		body.SetBinding("style.borderLeftWidth", BodyBorder_Width_Binding);
		body.SetBinding("style.borderRightWidth", BodyBorder_Width_Binding);
		body.SetBinding("style.borderBottomWidth", BodyBorder_Width_Binding);
	}


	// Although the camera transform itself is not used, we still use reactivity (not just a change notification)
	// because of its convenience
	private Vector2 Compute_PanelPosition(Transform _)
	{
		return RuntimePanelUtils.CameraTransformWorldToPanel(
			UIDocument.rootVisualElement.panel, _model.WorldPosition, _camera);
	}


	private static float Compute_Left(
		float coreRadius, float borderWidth,
		Vector2 panelAnchorPoint)
	{
		var halfWidth = coreRadius + borderWidth;
		return panelAnchorPoint.x - halfWidth;
	}


	private float Compute_Bottom(Vector2 panelPosition)
	{
		Assert.IsTrue(UIDocument.panelSettings.scaleMode == PanelScaleMode.ScaleWithScreenSize);

		return UIDocument.panelSettings.referenceResolution.y - panelPosition.y;
	}
}



}
