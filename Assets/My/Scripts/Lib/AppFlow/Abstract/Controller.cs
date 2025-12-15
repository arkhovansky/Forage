using System;
using System.Collections.Generic;

using Lib.AppFlow.Internal;



namespace Lib.AppFlow {



public abstract class Controller
	: IController,
	  IController_Internal
{
	protected readonly ICommand_Emitter CommandEmitter;

	protected readonly Dictionary<Type, Delegate> InputEvent_Handlers = new();
	protected readonly Dictionary<Type, Delegate> Command_Handlers = new();

	//----------------------------------------------------------------------------------------------


	protected Controller(ICommand_Emitter commandEmitter)
	{
		CommandEmitter = commandEmitter;
	}


	//----------------------------------------------------------------------------------------------
	// IController_Internal implementation


	IReadOnlyDictionary<Type, Delegate> IController_Internal.InputEvent_Handlers => InputEvent_Handlers;
	IReadOnlyDictionary<Type, Delegate> IController_Internal.Command_Handlers => Command_Handlers;


	//----------------------------------------------------------------------------------------------
	// IController implementation


	public virtual void Start() {}

	public virtual void Update() {}


	//----------------------------------------------------------------------------------------------
	// protected


	protected virtual void Add_InputEvent_Handler<TMessage>(Action<TMessage> method)
		where TMessage : IInputEvent
	{
		InputEvent_Handlers[typeof(TMessage)] = method;
	}

	protected virtual void Remove_InputEvent_Handler<TMessage>()
		where TMessage : IInputEvent
	{
		InputEvent_Handlers.Remove(typeof(TMessage));
	}


	protected virtual void Add_Command_Handler<TMessage>(Action<TMessage> method)
		where TMessage : ICommand
	{
		Command_Handlers[typeof(TMessage)] = method;
	}

	protected virtual void Remove_Command_Handler<TMessage>()
		where TMessage : ICommand
	{
		Command_Handlers.Remove(typeof(TMessage));
	}


	protected virtual void EmitCommand(ICommand command)
	{
		CommandEmitter.Emit(command);
	}
}



}
