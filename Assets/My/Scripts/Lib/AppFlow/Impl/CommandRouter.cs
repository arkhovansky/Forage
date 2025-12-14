using System;
using System.Collections.Generic;



namespace Lib.AppFlow.Impl {



public class CommandRouter : ICommandRouter
{
	private record EmittedCommand(
		ICommand Command,
		IContext Emitter
	);

	private readonly Queue<EmittedCommand> _commands = new ();


	private bool _commandEmittingBlocked;



	public void EmitCommand(ICommand command, IContext emitter)
	{
		if (_commandEmittingBlocked) {
			return;
		}

		_commands.Enqueue(new EmittedCommand(command, emitter));
	}



	private void HandleCommand(EmittedCommand command)
	{
		HandleCommand(command.Command, command.Emitter);
	}


	private void HandleCommand(ICommand command, IContext emitter)
	{
		IContext? context = emitter;
		bool searchInController = true;

		do {
			var handler = FindHandler(command.GetType(), context, searchInController);

			if (handler != null) {
				handler.DynamicInvoke(command);
				return;
			}

			context = context.Parent;
			searchInController = false;
		}
		while (context != null);
	}


	private Delegate? FindHandler(Type commandType, IContext context, bool searchInController)
	{
		Delegate handler;

		if (searchInController) {
			if (context.Controller != null
			    && context.Controller.CommandHandlers.TryGetValue(commandType, out handler))
			{
				return handler;
			}
		}

		if (context.CommandHandlers.TryGetValue(commandType, out handler))
			return handler;

		return null;
	}



	public void Update()
	{
		while (_commands.Count > 0) {
			EmittedCommand command = _commands.Dequeue();
			HandleCommand(command);
		}
	}
}



}
