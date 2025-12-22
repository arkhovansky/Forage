using System;
using System.Collections.Generic;

using Lib.AppFlow.Resolution.Internal;



namespace Lib.AppFlow.Resolution.Impl {



public class ContextCapability
	: IContextCapability,
	  IContextCapability_Internal,
	  IContextCapability_Builder
{
	private readonly Dictionary<string, object> _fields = new();

	private readonly HashSet<Type> _parameters = new();


	//----------------------------------------------------------------------------------------------
	// IContextCapability_Internal


	public IReadOnlyDictionary<string, object> Fields
		=> _fields;

	public IReadOnlyCollection<Type> Parameters
		=> _parameters;


	//----------------------------------------------------------------------------------------------
	// IContextCapability_Builder


	public IContextCapability_Builder Subject(string value)
	{
		_fields[FieldNames.Subject] = value;
		return this;
	}

	public IContextCapability_Builder Subject<T>()
		where T : class
	{
		_fields[FieldNames.Subject] = typeof(T);
		return this;
	}

	public IContextCapability_Builder Field(string name, object value)
	{
		_fields[name] = value;
		return this;
	}

	public IContextCapability_Builder Parameter<T>()
	{
		_parameters.Add(typeof(T));
		return this;
	}

	public IContextCapability Build()
	{
		if (!_fields.ContainsKey(FieldNames.Subject))
			throw new InvalidOperationException("Descriptor must have subject field");

		return this;
	}
}



}
