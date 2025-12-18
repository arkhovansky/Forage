using UnityEngine.Assertions;

using Lib.AppFlow;

using App.Game.Core;
using App.Game.Core.Query;
using App.Game.ECS.Camp.Components;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.GameTime.Components.Events;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class RunningGameInstance
	: IRunningGameInstance,
	  ILoopComponent
{
	public RunningGameInstance(IWorld world)
	{
		World = world;
		GamePhase = GamePhase.Arrival;

		Assert.IsFalse(EcsService.GameSystems_Enabled);
	}


	//----------------------------------------------------------------------------------------------
	// IRunningGameInstance_RO implementation


	IWorld_RO IRunningGameInstance_RO.World => World;

	public GamePhase GamePhase { get; private set; }


	//----------------------------------------------------------------------------------------------
	// IRunningGameInstance implementation


	public IWorld World { get; }


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
		switch (GamePhase) {
			case GamePhase.Arrival:
				if (CampExists())
					GamePhase = GamePhase.InterPeriod;
				break;

			case GamePhase.PeriodRunning:
				if (EcsService.IsEventRaised<YearPeriodChanged>())
					GamePhase = GamePhase.InterPeriod;
				break;
		}
	}


	//----------------------------------------------------------------------------------------------
	// private


	private bool CampExists()
	{
		return EcsService.SingletonExistsAnywhere<Camp>();
	}
}



}
