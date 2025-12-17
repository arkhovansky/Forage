using Lib.AppFlow;

using App.Application.Flow.GameInstance.RunningGame.Messages.PresentationEvents;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Views {



/// <summary>
/// View displaying presentation-related effects on the scene (like highlighting)
/// </summary>
public class ScenePresentationView : View
{
	public ScenePresentationView()
	{
		base.Add_PresentationEvent_Handler<HighlightedTile_Changed>(On_HighlightedTile_Changed);
	}


	//----------------------------------------------------------------------------------------------
	// Message handlers


	private void On_HighlightedTile_Changed(HighlightedTile_Changed evt)
	{
		EcsService.SendEcsCommand(new HoveredTileChanged_Event(evt.Position));
	}
}



}
