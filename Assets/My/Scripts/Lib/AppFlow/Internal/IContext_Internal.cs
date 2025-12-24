using System;
using System.Collections.Generic;

using Lib.AppFlow.Resolution;



namespace Lib.AppFlow.Internal {



public interface IContext_Internal
{
	void Init(IMessageDispatcher messageDispatcher,
	          IContextData contextData,
	          IContextHost contextHost);

	IReadOnlyDictionary<Type, Delegate> Command_Handlers { get; }

	IController? Controller { get; }

	IReadOnlyList<IView> Views { get; }

	IContextData ContextData { get; }
}



}
