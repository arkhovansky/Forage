using Lib.AppFlow;
using Lib.Grid;

using App.Application.Contexts.RunningGame.Messages.PresentationEvents;



namespace App.Application.Contexts.RunningGame.Models.UI.Impl {



public class RunningGame_UIModel
	: IRunningGame_UIModel,
	  ILoopComponent
{
	private readonly IPresentationEvent_Emitter _presentationEvent_Emitter;

	private AxialPosition? _highlightedTile;

	//----------------------------------------------------------------------------------------------


	public RunningGame_UIModel(IPresentationEvent_Emitter presentationEvent_Emitter)
	{
		_presentationEvent_Emitter = presentationEvent_Emitter;
	}


	//----------------------------------------------------------------------------------------------
	// IRunningGame_UIModel implementation


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


	//----------------------------------------------------------------------------------------------
	// ILoopComponent implementation


	void ILoopComponent.Start()
	{
		_presentationEvent_Emitter.Emit(new PositionCameraToOverview_Request());
	}
}



}
