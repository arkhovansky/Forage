using Lib.AppFlow;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.Shared;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationLayer {



public interface IResourceMarker
	: IResettableToResource,
	  ILoopComponent
{}



}
