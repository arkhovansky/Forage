using System;
using System.Collections.Generic;

using Lib.AppFlow.Internal;



namespace Lib.AppFlow {



public abstract class Controller
	: IController,
	  IController_Internal
{
	protected readonly IMessageEmitter MessageEmitter;

	// ReSharper disable once InconsistentNaming
	protected readonly Dictionary<Type, Delegate> commandHandlers = new();

	//----------------------------------------------------------------------------------------------


	protected Controller(IMessageEmitter messageEmitter)
	{
		MessageEmitter = messageEmitter;
	}


	//----------------------------------------------------------------------------------------------
	// IController_Internal implementation


	IReadOnlyDictionary<Type, Delegate> IController_Internal.CommandHandlers => commandHandlers;


	//----------------------------------------------------------------------------------------------
	// IController implementation


	public virtual void Start() {}

	public virtual void Update() {}


	//----------------------------------------------------------------------------------------------
	// protected


	protected virtual void AddCommandHandler<TCommand>(Action<TCommand> method)
	{
		commandHandlers[typeof(TCommand)] = method;
	}

	protected virtual void RemoveCommandHandler<TCommand>()
	{
		commandHandlers.Remove(typeof(TCommand));
	}


	protected virtual void EmitCommand(ICommand command)
	{
		MessageEmitter.EmitCommand(command);
	}
}



}
