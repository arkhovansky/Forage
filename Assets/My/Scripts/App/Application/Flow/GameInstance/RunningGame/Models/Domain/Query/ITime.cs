using App.Game.ECS.GameTime.Components;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface ITime
{
	GameTime Get_Time();


	bool Get_DayChanged();

	bool Get_YearPeriodChanged();


	bool Get_IsDaylight();
}



}
