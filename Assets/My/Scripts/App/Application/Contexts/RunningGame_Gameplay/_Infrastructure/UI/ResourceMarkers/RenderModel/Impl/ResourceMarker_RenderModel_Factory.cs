using AtomLifetime = UniMob.Lifetime;

using Lib.Grid.Spatial;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationModel;
using App.Infrastructure.Shared.Contracts.Database.Presentation.Types;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel.Impl {



public class ResourceMarker_RenderModel_Factory : IResourceMarker_RenderModel_Factory
{
	private readonly ResourceMarker_Parameters _markerParameters;

	private readonly HexGridLayout_3D _gridLayout;

	//----------------------------------------------------------------------------------------------


	public ResourceMarker_RenderModel_Factory(
		ResourceMarker_Parameters markerParameters,
		in HexGridLayout_3D gridLayout)
	{
		_markerParameters = markerParameters;
		_gridLayout = gridLayout;
	}


	public IResourceMarker_RenderModel Create(
		IResourceMarker_PresentationModel presentationModel,
		AtomLifetime atomLifetime)
	{
		return new ResourceMarker_RenderModel(
			presentationModel,
			_markerParameters,
			in _gridLayout,
			atomLifetime);
	}
}



}
