namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels {



public partial class RunningGameUI_VM
{
	private class CampPlacing_Mode : IMode
	{
		private readonly RunningGameUI_VM _context;



		public CampPlacing_Mode(RunningGameUI_VM context)
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
