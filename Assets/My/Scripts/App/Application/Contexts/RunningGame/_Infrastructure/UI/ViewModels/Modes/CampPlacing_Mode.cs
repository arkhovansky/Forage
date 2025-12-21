namespace App.Application.Contexts.RunningGame._Infrastructure.UI.ViewModels {



public partial class RunningGame_ScreenUI_VM
{
	private class CampPlacing_Mode : IMode
	{
		private readonly RunningGame_ScreenUI_VM _context;



		public CampPlacing_Mode(RunningGame_ScreenUI_VM context)
		{
			_context = context;
		}


		public void Enter()
		{
			_context.SelectCampLocationHintVM.IsVisible = true;
		}

		public void Exit()
		{
			_context.SelectCampLocationHintVM.IsVisible = false;
		}
	}
}



}
