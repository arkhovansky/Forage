namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationLayer {



public interface IResourceMarker_Pool
{
	IResourceMarker Acquire();

	void Release(IResourceMarker resourceMarker);
}



}
