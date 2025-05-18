using System.Collections.Generic;
using System.Linq;



namespace Lib.Util {



public static class EnumerableExtensions
{
	public static uint Sum(this IEnumerable<uint> source)
	{
		return source.Aggregate<uint, uint>(0, (current, u) => current + u);
	}
}



}
