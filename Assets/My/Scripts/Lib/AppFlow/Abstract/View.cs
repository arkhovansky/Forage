using System;
using System.Collections.Generic;

using Lib.AppFlow.Internal;



namespace Lib.AppFlow {



public abstract class View
	: IView,
	  IView_Internal
{
	protected readonly Dictionary<Type, Delegate> PresentationEvent_Handlers = new();


	//----------------------------------------------------------------------------------------------
	// IView_Internal implementation


	IReadOnlyDictionary<Type, Delegate> IView_Internal.PresentationEvent_Handlers => PresentationEvent_Handlers;


	//----------------------------------------------------------------------------------------------
	// IView implementation


	public virtual void Start() {}

	public virtual void Update() {}

	public virtual void LateUpdate() {}


	//----------------------------------------------------------------------------------------------
	// protected


	protected virtual void Add_PresentationEvent_Handler<TMessage>(Action<TMessage> method)
		where TMessage : IPresentationEvent
	{
		PresentationEvent_Handlers[typeof(TMessage)] = method;
	}

	protected virtual void Remove_PresentationEvent_Handler<TMessage>()
		where TMessage : IPresentationEvent
	{
		PresentationEvent_Handlers.Remove(typeof(TMessage));
	}
}



}
