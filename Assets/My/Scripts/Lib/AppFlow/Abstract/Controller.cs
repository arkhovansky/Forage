using System;
using System.Collections.Generic;



namespace Lib.AppFlow {



public abstract class Controller : IController
{
	public IReadOnlyDictionary<Type, Delegate> CommandHandlers => commandHandlers;


	protected readonly IMessageEmitter MessageEmitter;

	// ReSharper disable once InconsistentNaming
	protected readonly Dictionary<Type, Delegate> commandHandlers = new();

	//----------------------------------------------------------------------------------------------


	protected Controller(IMessageEmitter messageEmitter)
	{
		MessageEmitter = messageEmitter;
	}


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
