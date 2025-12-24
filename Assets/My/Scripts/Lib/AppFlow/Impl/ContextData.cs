using System;
using System.Collections.Generic;



namespace Lib.AppFlow.Impl {



public class ContextData : IContextData
{
	private readonly IContextData? _parent;

	private readonly Dictionary<Type, object> _objects = new();

	//----------------------------------------------------------------------------------------------


	public ContextData(IContextData? parent = null)
	{
		_parent = parent;
	}


	//----------------------------------------------------------------------------------------------
	// IContextData


	public void Add<T>(T obj)
		where T : notnull
	{
		_objects.Add(typeof(T), obj);
	}


	public T Get<T>()
		where T : notnull
	{
		if (_objects.TryGetValue(typeof(T), out var obj))
			return (T) obj;

		if (_parent != null)
			return _parent.Get<T>();

		throw new Exception($"Context data not found: {typeof(T)}");
	}
}



}
