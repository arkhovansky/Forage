using System;



namespace Lib.Grid {



public struct AxialVector : IEquatable<AxialVector>
{
	public int Q;
	public int R;


	public AxialVector(int q, int r)
	{
		Q = q;
		R = r;
	}


	public override bool Equals(object? obj)
		=> obj is AxialVector other && Equals(other);

	public bool Equals(AxialVector other)
		=> Q == other.Q && R == other.R;

	public override int GetHashCode() {
		unchecked {
			return (Q * 397) ^ R;  // Auto-generated
		}
	}

	public static bool operator ==(AxialVector left, AxialVector right)
		=> left.Equals(right);

	public static bool operator !=(AxialVector left, AxialVector right)
		=> !left.Equals(right);


	public static AxialVector operator +(AxialVector a, AxialVector b)
		=> new(a.Q + b.Q, a.R + b.R);

	public static AxialVector operator -(AxialVector a, AxialVector b)
		=> new(a.Q - b.Q, a.R - b.R);


	public static AxialVector operator *(AxialVector v, int scalar)
		=> new(v.Q * scalar, v.R * scalar);

	public static AxialVector operator *(int scalar, AxialVector v)
		=> new(v.Q * scalar, v.R * scalar);


	public override string ToString()
		=> $"({Q}, {R})";
}



}
