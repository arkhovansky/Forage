using AtomLifetime = UniMob.Lifetime;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.View {



public interface IResourceMarker_View_Pool
{
	IResourceMarker_View Acquire(
		IResourceMarker_RenderModel model,
		AtomLifetime atomLifetime);

	void Release(IResourceMarker_View view);
}



}
