using App.Services;



namespace App.Client.UI.GameInstance.RunningGame {



public partial class RunningGameController
{
	private class PeriodRunning_Mode : IMode
	{
		private readonly RunningGameController _controller;



		public PeriodRunning_Mode(RunningGameController controller)
		{
			_controller = controller;
		}


		public void Update()
		{
			if (EcsService.IsEventRaised<Game.ECS.GameTime.Components.Events.YearPeriodChanged>())
				_controller.EmitCommand(new YearPeriodChanged());
		}
	}
}



}
