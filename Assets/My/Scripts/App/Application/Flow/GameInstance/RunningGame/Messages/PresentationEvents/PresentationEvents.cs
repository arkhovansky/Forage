using Lib.AppFlow;
using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Messages.PresentationEvents {



public class PositionCameraToOverview_Request : IPresentationEvent {}



public record HighlightedTile_Changed(
	AxialPosition? Position
) : IPresentationEvent;



}
