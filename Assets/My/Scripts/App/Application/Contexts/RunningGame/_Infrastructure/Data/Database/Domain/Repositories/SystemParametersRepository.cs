using App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Domain.ScriptableObjects;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.ECS.BandMember.Gathering.Rules;
using App.Game.ECS.BandMember.Movement.Rules;
using App.Game.ECS.GameTime.Rules;



namespace App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Domain.Repositories {



public class SystemParametersRepository : ISystemParametersRepository
{
	private readonly SystemParameters _systemParameters_Asset;



	public SystemParametersRepository(SystemParameters systemParameters_Asset)
	{
		_systemParameters_Asset = systemParameters_Asset;
	}


	public GameTime_Rules Get_GameTime_Rules()
		=> _systemParameters_Asset.GameTime;

	public Daylight_Rules Get_Daylight_Rules()
		=> _systemParameters_Asset.Daylight;

	public Movement_Rules Get_Movement_Rules()
		=> _systemParameters_Asset.Movement;

	public Gathering_Rules Get_Gathering_Rules()
		=> _systemParameters_Asset.Gathering;
}



}
