namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels {



public partial class RunningGame_ScreenUI_VM
{
	private class InterPeriod_Mode : IMode
	{
		private readonly RunningGame_ScreenUI_VM _context;



		public InterPeriod_Mode(RunningGame_ScreenUI_VM context)
		{
			_context = context;
		}


		public void Enter()
		{
			_context.RunYearPeriodCommand.IsVisible = true;
		}

		public void Exit()
		{
			_context.RunYearPeriodCommand.IsVisible = false;
		}
	}
}



}
