using System;
using System.Collections.Generic;



namespace Lib.AppFlow.Internal {



public interface IController_Internal
{
	IReadOnlyDictionary<Type, Delegate> InputEvent_Handlers { get; }
	IReadOnlyDictionary<Type, Delegate> Command_Handlers { get; }
}



}
