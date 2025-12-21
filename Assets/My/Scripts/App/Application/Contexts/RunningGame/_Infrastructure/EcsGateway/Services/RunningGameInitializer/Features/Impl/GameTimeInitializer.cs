using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Services;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Components.Commands;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



public class GameTimeInitializer : IGameTimeInitializer
{
	private readonly IEcsHelper _ecsHelper;



	public GameTimeInitializer(IEcsHelper ecsHelper)
	{
		_ecsHelper = ecsHelper;
	}


	public void Init(YearPeriod yearPeriod)
	{
		_ecsHelper.SendEcsCommand(new InitYearPeriod(yearPeriod));
	}
}



}
