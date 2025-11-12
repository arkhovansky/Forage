using Lib.Grid;

using App.Application.Flow.GameInstance.RunningGame.Models;
using App.Game.ECS.Camp.Components.Commands;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.GameTime.Components.Events;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models_Impl {



public class RunningGameInstance : IRunningGameInstance
{
	public RunningGameInstance()
	{
		EcsService.GameSystems_Enabled = false;
	}


	public void Start()
	{
		EcsService.GameSystems_Enabled = true;
	}


	public void PlaceCamp(AxialPosition position)
	{
		EcsService.SendEcsCommand(new PlaceCamp(position));
	}


	public void RunYearPeriod()
	{
		EcsService.SendEcsCommand(new RunYearPeriod());
	}


	public bool IsYearPeriodChanged()
	{
		return EcsService.IsEventRaised<YearPeriodChanged>();
	}
}



}
