using UnityEngine;



namespace Lib.Grid {



public readonly struct Fractional_AxialPosition
{
	public readonly float Q;
	public readonly float R;

	//----------------------------------------------------------------------------------------------


	public Fractional_AxialPosition(float q, float r)
	{
		Q = q;
		R = r;
	}


	public AxialPosition Round()
	{
		float sFrac = - (Q + R);

		int q = Mathf.RoundToInt(Q);
		int r = Mathf.RoundToInt(R);
		int s = Mathf.RoundToInt(sFrac);

		float qDiff = Mathf.Abs(q - Q);
		float rDiff = Mathf.Abs(r - R);
		float sDiff = Mathf.Abs(s - sFrac);

		if (qDiff > rDiff && qDiff > sDiff) {
			q = -r - s;
		} else if (rDiff > sDiff) {
			r = -q - s;
		}

		return new AxialPosition(q, r);
	}


	public override string ToString()
		=> $"({Q}, {R})";


	//----------------------------------------------------------------------------------------------
	// static


	public static Fractional_AxialPosition Lerp(AxialPosition start, AxialPosition end, float t)
	{
		return new Fractional_AxialPosition(Lerp(start.Q, end.Q, t), Lerp(start.R, end.R, t));
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static float Lerp(int a, int b, float t)
	{
		return a + (b - a) * t;
	}
}



}
