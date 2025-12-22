using System;
using System.Collections.Generic;



namespace Lib.AppFlow.Resolution.Internal {



public interface IContextCapability_Internal
{
	IReadOnlyDictionary<string, object> Fields { get; }

	IReadOnlyCollection<Type> Parameters { get; }
}



}
