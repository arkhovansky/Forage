using Unity.Assertions;
using Unity.Entities;



namespace App.Game.ECS.GameTime.Components {



public struct GameTime : IComponentData
{
	public const uint DaysInYearPeriod = 2;



	public YearPeriod YearPeriod;

	public uint Day;

	public float Hours;

	public float DeltaHours;

	public bool DayChanged;



	public GameTime(YearPeriod yearPeriod, uint day, float hours)
	{
		YearPeriod = yearPeriod;
		Day = day;
		Hours = hours;

		DeltaHours = 0;
		DayChanged = false;
	}


	public void Advance(float deltaHours)
	{
		Assert.IsTrue(deltaHours <= 24);

		var previousDay = Day;

		DeltaHours = deltaHours;

		Hours += deltaHours;

		if (Hours >= 24) {
			Hours -= 24;
			++Day;
		}

		if (Day > DaysInYearPeriod) {
			Day = 1;
			YearPeriod.Advance();
		}

		DayChanged = (Day != previousDay);
	}


	public bool AdvanceTillNextYearPeriod(float deltaHours)
	{
		float hoursTillNextDay = 24 - Hours;
		var previousYearPeriod = YearPeriod;

		Advance(deltaHours);

		if (YearPeriod != previousYearPeriod) {
			Hours = 0;
			DeltaHours = hoursTillNextDay;

			return true;
		}

		return false;
	}
}



}
