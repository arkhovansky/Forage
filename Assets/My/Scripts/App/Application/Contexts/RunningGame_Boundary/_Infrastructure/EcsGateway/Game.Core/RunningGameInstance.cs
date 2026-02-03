using UnityEngine.Assertions;

using Lib.AppFlow;

using App.Game.Core;
using App.Game.Core.Query;
using App.Game.ECS.Camp.Components;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.GameTime.Components.Events;
using App.Infrastructure.EcsGateway.Contracts.Services;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Game.Core {



public class RunningGameInstance
	: IRunningGameInstance,
	  ILoopComponent
{
	private readonly IEcsSystems_Service _ecsSystems_Service;
	private readonly IEcsHelper _ecsHelper;

	//----------------------------------------------------------------------------------------------


	public RunningGameInstance(IWorld world,
	                           IEcsSystems_Service ecsSystems_Service,
	                           IEcsHelper ecsHelper)
	{
		World = world;
		GamePhase = GamePhase.Arrival;

		_ecsSystems_Service = ecsSystems_Service;
		_ecsHelper = ecsHelper;

		Assert.IsFalse(_ecsSystems_Service.GameSystems_Enabled);
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
		_ecsHelper.SendEcsCommand(new RunYearPeriod());
		GamePhase = GamePhase.PeriodRunning;
	}


	//----------------------------------------------------------------------------------------------
	// ILoopComponent implementation


	void ILoopComponent.Start()
	{
		_ecsSystems_Service.GameSystems_Enabled = true;
	}


	void ILoopComponent.LateUpdate()
	{
		switch (GamePhase) {
			case GamePhase.Arrival:
				if (CampExists())
					GamePhase = GamePhase.InterPeriod;
				break;

			case GamePhase.PeriodRunning:
				if (_ecsHelper.IsEventRaised<YearPeriodChanged>())
					GamePhase = GamePhase.InterPeriod;
				break;
		}
	}


	//----------------------------------------------------------------------------------------------
	// private


	private bool CampExists()
	{
		return _ecsHelper.HasSingleton_Anywhere<Camp>();
	}
}



}
