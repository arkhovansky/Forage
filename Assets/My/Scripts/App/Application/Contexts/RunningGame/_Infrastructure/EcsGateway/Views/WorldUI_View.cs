using Lib.AppFlow;

using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Services;
using App.Application.Contexts.RunningGame.Messages.PresentationEvents;
using App.Game.ECS.UI.HighlightedTile.Components;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Views {



/// <summary>
/// View for the world-space UI (like highlighting)
/// </summary>
public class WorldUI_View : View
{
	private readonly IEcsHelper _ecsHelper;

	//----------------------------------------------------------------------------------------------


	public WorldUI_View(IEcsHelper ecsHelper)
	{
		_ecsHelper = ecsHelper;

		base.Add_PresentationEvent_Handler<HighlightedTile_Changed>(On_HighlightedTile_Changed);
	}


	//----------------------------------------------------------------------------------------------
	// Message handlers


	private void On_HighlightedTile_Changed(HighlightedTile_Changed evt)
	{
		_ecsHelper.SendEcsCommand(new HighlightedTile_Changed_Event(evt.Position));
	}
}



}
