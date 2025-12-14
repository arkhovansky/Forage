namespace Lib.AppFlow {



public interface ICommandRouter
{
	void EmitCommand(ICommand command, IContext emitter);

	void Update();
}



}
