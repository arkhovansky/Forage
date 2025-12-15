namespace Lib.AppFlow {



public interface ICommandDispatcher
{
	void EmitCommand(ICommand command, IContext emitter);

	void Update();
}



}
