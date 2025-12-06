using System;



namespace App.Game.Meta {



public readonly struct LocaleId : IEquatable<LocaleId>
{
	private readonly string _value;


	public LocaleId(string value)
	{
		_value = value;
	}


	public bool Equals(LocaleId other)
	{
		return _value == other._value;
	}

	public override bool Equals(object? obj)
	{
		return obj is LocaleId other && Equals(other);
	}

	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	public static bool operator ==(LocaleId left, LocaleId right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(LocaleId left, LocaleId right)
	{
		return !left.Equals(right);
	}

	public static implicit operator string(LocaleId x)
		=> x._value;
}



}
