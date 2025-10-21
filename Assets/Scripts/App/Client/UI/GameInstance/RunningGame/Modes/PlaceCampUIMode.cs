namespace App.Client.UI.GameInstance.RunningGame {



public partial class RunningGameController
{
	private class PlaceCampUIMode : IUIMode
	{
		private readonly RunningGameController _controller;



		public PlaceCampUIMode(RunningGameController controller)
		{
			_controller = controller;
		}


		public void OnEnter()
		{
			_controller.AddCommandHandler<TileClicked>(OnTileClicked);
		}

		public void OnExit()
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
