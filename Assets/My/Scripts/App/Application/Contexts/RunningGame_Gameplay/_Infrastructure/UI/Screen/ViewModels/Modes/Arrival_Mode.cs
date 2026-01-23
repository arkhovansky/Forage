namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.Screen.ViewModels {



public partial class RunningGame_ScreenUI_VM
{
	private class Arrival_Mode : IMode
	{
		private readonly RunningGame_ScreenUI_VM _context;



		public Arrival_Mode(RunningGame_ScreenUI_VM context)
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
