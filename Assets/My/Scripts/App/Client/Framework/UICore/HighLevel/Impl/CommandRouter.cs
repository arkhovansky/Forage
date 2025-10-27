using System.Collections.Generic;



namespace App.Client.Framework.UICore.HighLevel.Impl {



public class CommandRouter : ICommandRouter
{
	// private IController? _rootController;
	//
	//
	// private record CommandHandler(
	// 	IController Controller,
	// 	Delegate Action
	// );
	//
	// private readonly Dictionary<Type, CommandHandler> _commandHandlers;


	private record EmittedCommand(
		ICommand Command,
		IController Emitter
	);

	private readonly Queue<EmittedCommand> _commands = new ();

	// private EmittedCommand? _processedCommand;


	private bool _commandEmittingBlocked;



	public CommandRouter()
	{
	}



	// public void SetRootController(IController controller)
	// {
	// 	_rootController = controller;
	//
	// 	RegisterCommands(_rootController);
	//
	// 	if (_rootVisualNode != null)
	// 		_rootController.SetVisualNode(_rootVisualNode);
	// }



	// private void RegisterCommands(IController controller)
	// {
	// 	foreach (var kv in controller.CommandsHandlers) {
	// 		if (_commandHandlers.ContainsKey(kv.Key))
	// 			throw new InvalidOperationException(); //TODO
	//
	// 		_commandHandlers[kv.Key] = new CommandHandler(controller, kv.Value);
	// 	}
	//
	// 	foreach (var child in controller.Children)
	// 		RegisterCommands(child);
	// }


	public void AddController(IController controller)
	{
		//TODO
	}


	public void RemoveController(IController controller)
	{
		//TODO
	}



	public void EmitCommand(ICommand command, IController emitter)
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


	private void HandleCommand(ICommand command, IController emitter)
	{
		IController? controller = emitter;
		do {
			if (controller.CommandHandlers.TryGetValue(command.GetType(), out var handler)) {
				handler.DynamicInvoke(command);

				return;
			}

			controller = controller.Parent;
		}
		while (controller != null);
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
