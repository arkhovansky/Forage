using Lib.AppFlow;
using Lib.Grid;

using App.Application.Flow.GameInstance.RunningGame.Messages.PresentationEvents;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Presentation.Impl {



public class RunningGame_PresentationModel : IRunningGame_PresentationModel
{
	private readonly IPresentationEvent_Emitter _presentationEvent_Emitter;

	private AxialPosition? _highlightedTile;

	//----------------------------------------------------------------------------------------------


	public RunningGame_PresentationModel(IPresentationEvent_Emitter presentationEvent_Emitter)
	{
		_presentationEvent_Emitter = presentationEvent_Emitter;
	}


	//----------------------------------------------------------------------------------------------
	// IRunningGame_PresentationModel implementation


	public AxialPosition? HighlightedTile {
		get => _highlightedTile;

		set {
			if (_highlightedTile == value)
				return;

			_highlightedTile = value;
			_presentationEvent_Emitter.Emit(new HighlightedTile_Changed(value));
		}
	}


	public bool Is_CampPlacing_Mode { get; set; } = false;
}



}
