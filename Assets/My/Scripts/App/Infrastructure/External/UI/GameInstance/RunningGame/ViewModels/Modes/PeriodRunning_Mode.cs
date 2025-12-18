namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels {



public partial class RunningGame_ScreenUI_VM
{
	private class PeriodRunning_Mode : IMode
	{
		private readonly RunningGame_ScreenUI_VM _context;



		public PeriodRunning_Mode(RunningGame_ScreenUI_VM context)
		{
			_context = context;
		}


		public void Exit()
		{
			_context.UpdateSimulationData();
		}


		public void Update()
		{
			_context.UpdateSimulationData();
		}
	}
}



}
