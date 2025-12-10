using App.Game.ECS.GameTime.Components;



namespace App.Game.Core.Query {



public interface ITime
{
	GameTime Get_Time();


	bool Get_DayChanged();

	bool Get_YearPeriodChanged();


	bool Get_IsDaylight();
}



}
