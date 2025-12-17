using UnityEngine.Assertions;

using Lib.AppFlow;
using Lib.Grid;

using App.Game.Core;
using App.Game.Core.Query;
using App.Game.ECS.Camp.Components.Commands;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.GameTime.Components.Events;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class RunningGameInstance
	: IRunningGameInstance,
	  ILoopComponent
{
	public RunningGameInstance(IWorld_RO world)
	{
		World = world;
		GamePhase = GamePhase.Arrival;

		Assert.IsFalse(EcsService.GameSystems_Enabled);
	}


	//----------------------------------------------------------------------------------------------
	// IRunningGameInstance_RO implementation


	public IWorld_RO World { get; }

	public GamePhase GamePhase { get; private set; }


	//----------------------------------------------------------------------------------------------
	// IRunningGameInstance implementation


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


	//----------------------------------------------------------------------------------------------
	// ILoopComponent implementation


	void ILoopComponent.Start()
	{
		EcsService.GameSystems_Enabled = true;
	}


	void ILoopComponent.LateUpdate()
	{
		if (GamePhase == GamePhase.PeriodRunning) {
			if (EcsService.IsEventRaised<YearPeriodChanged>())
				GamePhase = GamePhase.InterPeriod;
		}
	}
}



}
