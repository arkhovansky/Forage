using System;
using System.Collections.Generic;



namespace Lib.AppFlow.Internal {



public interface IView_Internal
{
	IReadOnlyDictionary<Type, Delegate> PresentationEvent_Handlers { get; }
}



}
