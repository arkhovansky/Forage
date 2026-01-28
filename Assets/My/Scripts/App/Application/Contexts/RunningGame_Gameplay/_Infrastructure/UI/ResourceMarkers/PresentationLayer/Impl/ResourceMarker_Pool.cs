using System;
using System.Collections.Generic;

using AtomLifetime = UniMob.Lifetime;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationModel.Impl;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.View;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationLayer.Impl {



// Although this class holds disposable nested atom lifetimes, it is not a disposable itself. This is because during
// context (and this object) shutdown all nested atom lifetimes are disposed automatically.
//
// Currently, there is no actual pooling.

public class ResourceMarker_Pool : IResourceMarker_Pool
{
	private readonly IResourceMarker_RenderModel_Factory _renderModel_Factory;
	private readonly IResourceMarker_View_Pool _view_Pool;

	private readonly AtomLifetime _atomLifetime;

	private readonly Dictionary<IResourceMarker, IDisposable> _markerAtomDisposers = new();

	//----------------------------------------------------------------------------------------------


	public ResourceMarker_Pool(
		IResourceMarker_RenderModel_Factory renderModel_Factory,
		IResourceMarker_View_Pool view_Pool,
		AtomLifetime atomLifetime)
	{
		_renderModel_Factory = renderModel_Factory;
		_view_Pool = view_Pool;
		_atomLifetime = atomLifetime;
	}


	//----------------------------------------------------------------------------------------------
	// IResourceMarker_Pool


	public IResourceMarker Acquire()
	{
		var atomLifetimeDisposer = _atomLifetime.CreateNested(out var nestedLifetime);

		var presentationModel = new ResourceMarker_PresentationModel(nestedLifetime);
		var renderModel = _renderModel_Factory.Create(presentationModel, nestedLifetime);
		var view = _view_Pool.Acquire(renderModel, nestedLifetime);

		var marker = new ResourceMarker(
			presentationModel,
			renderModel,
			view);

		_markerAtomDisposers.Add(marker, atomLifetimeDisposer);

		return marker;
	}


	public void Release(IResourceMarker marker)
	{
		var marker_ForPool = (IResourceMarker_ForPool) marker;
		_view_Pool.Release(marker_ForPool.View);

		_markerAtomDisposers.Remove(marker, out var disposer);
		disposer.Dispose();
	}
}



}
