using AtomLifetime = UniMob.Lifetime;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationModel;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel {



public interface IResourceMarker_RenderModel_Factory
{
	IResourceMarker_RenderModel Create(
		IResourceMarker_PresentationModel presentationModel,
		AtomLifetime atomLifetime);
}



}
