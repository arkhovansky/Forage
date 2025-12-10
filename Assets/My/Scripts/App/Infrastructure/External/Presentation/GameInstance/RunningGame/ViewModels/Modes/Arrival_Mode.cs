namespace App.Infrastructure.External.Presentation.GameInstance.RunningGame.ViewModels {



public partial class RunningGameUI_VM
{
	private class Arrival_Mode : IMode
	{
		private readonly RunningGameUI_VM _context;



		public Arrival_Mode(RunningGameUI_VM context)
		{
			_context = context;
		}


		public void Enter()
		{
			_context.EnterPlaceCampModeCommand.IsVisible = true;
			_context.UpdateSimulationData();
		}

		public void Exit()
		{
			_context.EnterPlaceCampModeCommand.IsVisible = false;
		}
	}
}



}
