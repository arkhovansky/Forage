using App.Game.ECS.BandMember.Gathering.Rules;
using App.Game.ECS.BandMember.Movement.Rules;
using App.Game.ECS.GameTime.Rules;
using App.Infrastructure.EcsGateway.Database.Domain;
using App.Infrastructure.External.Database.Domain.ScriptableObjects;



namespace App.Infrastructure.External.Database.Domain.Repositories {



public class SystemParametersRepository : ISystemParametersRepository
{
	private readonly SystemParameters _systemParameters
		= GameDatabase.Instance.Domain.SystemParameters;



	public GameTime_Rules Get_GameTime_Rules()
		=> _systemParameters.GameTime;

	public Daylight_Rules Get_Daylight_Rules()
		=> _systemParameters.Daylight;

	public Movement_Rules Get_Movement_Rules()
		=> _systemParameters.Movement;

	public Gathering_Rules Get_Gathering_Rules()
		=> _systemParameters.Gathering;
}



}
