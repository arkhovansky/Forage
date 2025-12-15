using App.Application.Flow.GameInstance.RunningGame.Messages.Commands;
using App.Application.Flow.GameInstance.RunningGame.Messages.InputEvents;



namespace App.Application.Flow.GameInstance.RunningGame.Controller {



public partial class RunningGameController
{
	private class CampPlacing_Mode : IMode
	{
		private readonly RunningGameController _controller;



		public CampPlacing_Mode(RunningGameController controller)
		{
			_controller = controller;
		}


		public void Enter()
		{
			_controller.Add_InputEvent_Handler<TileClicked>(OnTileClicked);
		}

		public void Exit()
		{
			_controller.Remove_InputEvent_Handler<TileClicked>();
		}


		private void OnTileClicked(TileClicked evt)
		{
			_controller.EmitCommand(new PlaceCamp(evt.Position));
		}
	}
}



}
