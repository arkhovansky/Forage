namespace App.Game.ECS.GameTime.Rules {



public static class Daylight_Rules
{
	private const int DaylightBeginHours = 5;
	private const int DaylightEndHours = 21;



	public static bool IsDaylight(in Components.GameTime gameTime)
	{
		return gameTime.IntegerHours is >= DaylightBeginHours and < DaylightEndHours;
	}


	public static bool GetDaylightEvent(in Components.GameTime gameTime, out bool isDaylight)
	{
		isDaylight = IsDaylight(gameTime);
		return gameTime is {IntegerHoursChanged: true, IntegerHours: DaylightBeginHours or DaylightEndHours};
	}
}



}
