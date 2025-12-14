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
			_controller.AddCommandHandler<TileClicked>(OnTileClicked);
		}

		public void Exit()
		{
			_controller.RemoveCommandHandler<TileClicked>();
		}


		private void OnTileClicked(TileClicked evt)
		{
			_controller.EmitCommand(new PlaceCamp(evt.Position));
		}
	}
}



}
