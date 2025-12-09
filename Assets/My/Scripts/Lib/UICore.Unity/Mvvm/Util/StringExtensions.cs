namespace Lib.UICore.Unity.Mvvm.Util {



public static class StringExtensions
{
	public static string TrimSuffix(this string s, string suffix)
	{
		if (!s.EndsWith(suffix))
			return s;

		return s.Remove(s.Length - suffix.Length);
	}
}



}
