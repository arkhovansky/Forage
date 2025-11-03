using App.Client.UI.GameInstance.RunningGame.ViewModels;



namespace App.Client.UI.GameInstance.RunningGame {



public partial class RunningGameController
{
	private class InterPeriod_Mode : IMode
	{
		private readonly RunYearPeriod_CommandVM _runYearPeriod_CommandVM;



		public InterPeriod_Mode(RunningGameController controller)
		{
			_runYearPeriod_CommandVM = controller._viewModel.RunYearPeriodCommand;
		}


		public void Enter()
		{
			_runYearPeriod_CommandVM.IsVisible = true;
		}

		public void Exit()
		{
			_runYearPeriod_CommandVM.IsVisible = false;
		}
	}
}



}
