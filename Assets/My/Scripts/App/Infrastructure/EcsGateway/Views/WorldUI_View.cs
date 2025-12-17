using Lib.AppFlow;

using App.Application.Flow.GameInstance.RunningGame.Messages.PresentationEvents;
using App.Game.ECS.UI.HighlightedTile.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Views {



/// <summary>
/// View for the world-space UI (like highlighting)
/// </summary>
public class WorldUI_View : View
{
	public WorldUI_View()
	{
		base.Add_PresentationEvent_Handler<HighlightedTile_Changed>(On_HighlightedTile_Changed);
	}


	//----------------------------------------------------------------------------------------------
	// Message handlers


	private void On_HighlightedTile_Changed(HighlightedTile_Changed evt)
	{
		EcsService.SendEcsCommand(new HighlightedTile_Changed_Event(evt.Position));
	}
}



}
