using System.Collections;
using System.Collections.Generic;



namespace Lib.Util {



public class SetList<T> : IReadOnlyList<T>
{
	public int Add(T item)
	{
		var index = _list.IndexOf(item);
		if (index != -1)
			return index;

		_list.Add(item);
		return _list.Count - 1;
	}


	public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

	public int Count => _list.Count;

	public T this[int index] => _list[index];


	//----------------------------------------------------------------------------------------------
	// private

	private readonly List<T> _list = new();
}



}
