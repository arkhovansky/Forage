using Lib.AppFlow;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationModel;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.Shared;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.View;
using App.Game.Core.Query;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationLayer.Impl {



public class ResourceMarker
	: IResourceMarker,
	  IResourceMarker_ForPool
{
	private readonly ILoopComponent _presentationModel;

	private readonly IResettable _renderModel;

	private readonly IResourceMarker_View _view;

	//----------------------------------------------------------------------------------------------


	public ResourceMarker(
		IResourceMarker_PresentationModel presentationModel,
		IResourceMarker_RenderModel renderModel,
		IResourceMarker_View view)
	{
		_presentationModel = (ILoopComponent) presentationModel;
		_renderModel = (IResettable) renderModel;
		_view = view;
	}


	//----------------------------------------------------------------------------------------------
	// IResettableToResource


	public void Reset(IPlantResource resource)
	{
		((IResettableToResource) _presentationModel).Reset(resource);
		_renderModel.Reset();
		_view.Reset();
	}


	//----------------------------------------------------------------------------------------------
	// ILoopComponent


	public void LateUpdate()
	{
		_presentationModel.LateUpdate();
	}


	//----------------------------------------------------------------------------------------------
	// IResourceMarker_ForPool


	IResourceMarker_View IResourceMarker_ForPool.View
		=> _view;
}



}
