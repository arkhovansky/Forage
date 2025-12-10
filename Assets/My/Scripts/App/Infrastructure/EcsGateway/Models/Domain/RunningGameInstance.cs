using UnityEngine.Assertions;

using Lib.Grid;

using App.Application.Flow.GameInstance.RunningGame.Models.Domain;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Game.ECS.Camp.Components.Commands;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.GameTime.Components.Events;
using App.Game.Models;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class RunningGameInstance : IRunningGameInstance
{
	public IWorld_RO World { get; }

	public GamePhase GamePhase { get; private set; }

	//----------------------------------------------------------------------------------------------


	public RunningGameInstance(IWorld_RO world)
	{
		World = world;
		GamePhase = GamePhase.Arrival;

		Assert.IsFalse(EcsService.GameSystems_Enabled);
	}


	public void Start()
	{
		EcsService.GameSystems_Enabled = true;
	}


	public void PlaceCamp(AxialPosition position)
	{
		EcsService.SendEcsCommand(new PlaceCamp(position));
		GamePhase = GamePhase.InterPeriod;
	}


	public void RunYearPeriod()
	{
		EcsService.SendEcsCommand(new RunYearPeriod());
		GamePhase = GamePhase.PeriodRunning;
	}


	public bool IsYearPeriodChanged()
	{
		bool yearPeriodChanged = EcsService.IsEventRaised<YearPeriodChanged>();

		if (yearPeriodChanged)
			GamePhase = GamePhase.InterPeriod;

		return yearPeriodChanged;
	}
}



}
