using System;
using System.Collections.Generic;



namespace Lib.AppFlow.Internal {



public interface IContext_Internal
{
	IReadOnlyDictionary<Type, Delegate> CommandHandlers { get; }

	IController? Controller { get; }
}



}
