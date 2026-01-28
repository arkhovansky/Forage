using UnityEngine;
using UnityEngine.UIElements;

using UniMob;
using AtomLifetime = UniMob.Lifetime;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.View.Impl {



// Currently, there is no actual pooling

public class ResourceMarker_View_Pool : IResourceMarker_View_Pool
{
	private readonly GameObject _markerPrefab;

	private readonly IResourceType_Icon_Repository _resourceType_Icon_Repository;

	private readonly Camera _camera;
	private readonly Atom<Transform> _cameraTransform_Atom;

	//----------------------------------------------------------------------------------------------


	public ResourceMarker_View_Pool(
		GameObject markerPrefab,
		IResourceType_Icon_Repository resourceTypeIconRepository,
		Camera camera,
		Atom<Transform> cameraTransform_Atom)
	{
		_markerPrefab = markerPrefab;
		_resourceType_Icon_Repository = resourceTypeIconRepository;
		_camera = camera;
		_cameraTransform_Atom = cameraTransform_Atom;
	}


	//----------------------------------------------------------------------------------------------
	// IResourceMarker_View_Pool


	public IResourceMarker_View Acquire(
		IResourceMarker_RenderModel model,
		AtomLifetime atomLifetime)
	{
		var gameObject = Object.Instantiate(_markerPrefab);
		var uiDocument = gameObject.GetComponent<UIDocument>();

		return new ResourceMarker_View(
			model,
			uiDocument,
			_resourceType_Icon_Repository,
			_camera, _cameraTransform_Atom,
			atomLifetime);
	}


	public void Release(IResourceMarker_View view)
	{
		var view_ForPool = (IResourceMarker_View_ForPool) view;
		Object.Destroy(view_ForPool.UIDocument.gameObject);
	}
}



}
