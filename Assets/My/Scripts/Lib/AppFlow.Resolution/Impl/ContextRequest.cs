using System;
using System.Collections.Generic;

using Lib.AppFlow.Resolution.Internal;



namespace Lib.AppFlow.Resolution.Impl {



public class ContextRequest
	: IContextRequest,
	  IContextRequest_Internal,
	  IContextRequest_Builder
{
	private readonly Dictionary<string, object> _fields = new();

	private readonly List<object> _arguments = new();


	//----------------------------------------------------------------------------------------------
	// IContextRequest


	public T GetSubject<T>()
	{
		return (T) _fields[FieldNames.Subject];
	}


	public T GetArgument<T>()
	{
		foreach (var argument in _arguments) {
			if (argument is T argument_T)
				return argument_T;
		}

		throw new ArgumentOutOfRangeException();
	}


	//----------------------------------------------------------------------------------------------
	// IContextRequest_Internal


	public IReadOnlyDictionary<string, object> Fields
		=> _fields;

	public IReadOnlyCollection<object> Arguments
		=> _arguments;


	//----------------------------------------------------------------------------------------------
	// IContextRequest_Builder


	public IContextRequest_Builder Subject(string value)
	{
		return Field(FieldNames.Subject, value);
	}

	public IContextRequest_Builder Subject<T>(T value)
		where T : class
	{
		return Field(FieldNames.Subject, value);
	}

	public IContextRequest_Builder Field(string name, object value)
	{
		_fields[name] = value;
		return this;
	}

	public IContextRequest_Builder Argument(object value)
	{
		_arguments.Add(value);
		return this;
	}

	public IContextRequest Build()
	{
		if (!_fields.ContainsKey(FieldNames.Subject))
			throw new InvalidOperationException("Descriptor must have subject field");

		return this;
	}
}



}
