namespace App.Infrastructure.External.Presentation.GameInstance.RunningGame.ViewModels {



public partial class RunningGameUI_VM
{
	private class PeriodRunning_Mode : IMode
	{
		private readonly RunningGameUI_VM _context;



		public PeriodRunning_Mode(RunningGameUI_VM context)
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
