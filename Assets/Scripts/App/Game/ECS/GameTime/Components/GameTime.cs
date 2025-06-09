using Unity.Assertions;
using Unity.Entities;

using UnityEngine;



namespace App.Game.ECS.GameTime.Components {



public struct GameTime : IComponentData
{
	public const uint DaysInYearPeriod = 5;



	public YearPeriod YearPeriod;

	public uint Day;

	public float Hours;

	public int IntegerHours;

	public float DeltaHours;


	public bool YearPeriodChanged;

	public bool DayChanged;

	public bool IntegerHoursChanged;



	public GameTime(YearPeriod yearPeriod, uint day, float hours)
	{
		YearPeriod = yearPeriod;
		Day = day;
		Hours = hours;
		IntegerHours = (int) hours;

		DeltaHours = 0;
		YearPeriodChanged = false;
		DayChanged = false;
		IntegerHoursChanged = false;
	}


	public void Advance(float deltaHours)
	{
		Assert.IsTrue(deltaHours <= 24);

		int previousIntHours = IntegerHours;

		DeltaHours = deltaHours;

		Hours += deltaHours;

		// Fix up accumulating error
		int intHours = Mathf.RoundToInt(Hours);
		// Do not use Mathf.Approximately(), the error can be larger than default Epsilon
		float epsilon = deltaHours / 100;  // Assuming one hour is multiple of deltaHours
		if (Mathf.Abs(Hours - intHours) < epsilon)
			Hours = intHours;

		IntegerHours = (int) Hours;

		IntegerHoursChanged = IntegerHours > previousIntHours;

		if (Hours >= 24) {
			Hours -= 24;
			++Day;
			DayChanged = true;
		}
		else
			DayChanged = false;

		if (Day > DaysInYearPeriod) {
			Day = 1;
			YearPeriod.Advance();
			YearPeriodChanged = true;
		}
		else
			YearPeriodChanged = false;
	}
}



}
