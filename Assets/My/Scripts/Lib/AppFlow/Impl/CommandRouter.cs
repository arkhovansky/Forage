using System.Collections.Generic;



namespace Lib.AppFlow.Impl {



public class CommandRouter : ICommandRouter
{
	private record EmittedCommand(
		ICommand Command,
		IController Emitter
	);

	private readonly Queue<EmittedCommand> _commands = new ();


	private bool _commandEmittingBlocked;



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
