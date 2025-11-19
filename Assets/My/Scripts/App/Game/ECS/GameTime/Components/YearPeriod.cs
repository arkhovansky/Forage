using System;



namespace App.Game.ECS.GameTime.Components {



public enum Month
{
	January = 1,
	February,
	March,
	April,
	May,
	June,
	July,
	August,
	September,
	October,
	November,
	December
}



[Serializable]
public struct YearPeriod : IEquatable<YearPeriod>
{
	public Month Month;



	public YearPeriod(Month month)
	{
		Month = month;
	}


	public void Advance()
	{
		if (Month < Month.December)
			++Month;
		else
			Month = Month.January;
	}


	public override bool Equals(object? obj)
		=> obj is YearPeriod other && Equals(other);

	public bool Equals(YearPeriod other)
		=> Month == other.Month;

	public override int GetHashCode()
		=> (int) Month;

	public static bool operator ==(YearPeriod left, YearPeriod right)
		=> left.Equals(right);

	public static bool operator !=(YearPeriod left, YearPeriod right)
		=> !left.Equals(right);
}



}
