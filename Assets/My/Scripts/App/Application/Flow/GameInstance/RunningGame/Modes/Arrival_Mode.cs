using App.Application.Flow.GameInstance.RunningGame.ViewModels;



namespace App.Application.Flow.GameInstance.RunningGame {



public partial class RunningGameController
{
	private class Arrival_Mode : IMode
	{
		private readonly EnterPlaceCampMode_CommandVM _enterPlaceCampMode_CommandVM;



		public Arrival_Mode(RunningGameController controller)
		{
			_enterPlaceCampMode_CommandVM = controller._viewModel.EnterPlaceCampModeCommand;
		}


		public void Enter()
		{
			_enterPlaceCampMode_CommandVM.IsVisible = true;
		}

		public void Exit()
		{
			_enterPlaceCampMode_CommandVM.IsVisible = false;
		}
	}
}



}
