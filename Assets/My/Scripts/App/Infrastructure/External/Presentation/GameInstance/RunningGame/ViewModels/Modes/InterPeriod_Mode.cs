namespace App.Infrastructure.External.Presentation.GameInstance.RunningGame.ViewModels {



public partial class RunningGameUI_VM
{
	private class InterPeriod_Mode : IMode
	{
		private readonly RunningGameUI_VM _context;



		public InterPeriod_Mode(RunningGameUI_VM context)
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
