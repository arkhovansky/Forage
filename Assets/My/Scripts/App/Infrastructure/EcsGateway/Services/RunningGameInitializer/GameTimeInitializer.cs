using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Commands;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer {



public class GameTimeInitializer : IGameTimeInitializer
{
	public void Init(YearPeriod yearPeriod)
	{
		EcsService.SendEcsCommand(new InitYearPeriod(yearPeriod));
	}
}



}
