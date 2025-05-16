using System;



namespace Lib.Grid {



public struct AxialPosition : IEquatable<AxialPosition>
{
	public int Q;
	public int R;


	public AxialPosition(int q, int r)
	{
		Q = q;
		R = r;
	}


	public override bool Equals(object? obj)
		=> obj is AxialPosition other && Equals(other);

	public bool Equals(AxialPosition other)
		=> Q == other.Q && R == other.R;

	public override int GetHashCode() {
		unchecked {
			return (Q * 397) ^ R;  // Auto-generated
		}
	}

	public static bool operator ==(AxialPosition left, AxialPosition right)
		=> left.Equals(right);

	public static bool operator !=(AxialPosition left, AxialPosition right)
		=> !left.Equals(right);


	public static AxialPosition operator +(AxialPosition a, AxialPosition b)
		=> new AxialPosition(a.Q + b.Q, a.R + b.R);

	public static AxialPosition operator -(AxialPosition a, AxialPosition b)
		=> new AxialPosition(a.Q - b.Q, a.R - b.R);


	public override string ToString()
		=> $"({Q}, {R})";
}



}
