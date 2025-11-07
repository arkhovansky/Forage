using App.Game;
using App.Game.ECS.GameTime.Components.Commands;
using App.Services;



namespace App.Infrastructure.ECS.Services.RunningGameInitializer_Impl {



public class GameTimeInitializer : IGameTimeInitializer
{
	public void Init(YearPeriod yearPeriod)
	{
		EcsService.SendEcsCommand(new InitYearPeriod(yearPeriod));
	}
}



}
