using App.Game.ECS.BandMember.Gathering.Rules;
using App.Game.ECS.BandMember.Movement.Rules;
using App.Game.ECS.GameTime.Rules;



namespace App.Game.Database {



public interface ISystemParametersRepository
{
	GameTime_Rules Get_GameTime_Rules();

	Daylight_Rules Get_Daylight_Rules();

	Movement_Rules Get_Movement_Rules();

	Gathering_Rules Get_Gathering_Rules();
}



}
