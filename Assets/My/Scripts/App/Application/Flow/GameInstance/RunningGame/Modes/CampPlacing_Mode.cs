namespace App.Application.Flow.GameInstance.RunningGame {



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
			_controller._viewModel.SelectCampLocationHintVM.IsVisible = true;
			_controller.AddCommandHandler<TileClicked>(OnTileClicked);
		}

		public void Exit()
		{
			_controller._viewModel.SelectCampLocationHintVM.IsVisible = false;
			_controller.RemoveCommandHandler<TileClicked>();
		}


		private void OnTileClicked(TileClicked evt)
		{
			_controller.EmitCommand(new PlaceCamp(evt.Position));
		}
	}
}



}
