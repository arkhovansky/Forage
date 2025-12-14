namespace Lib.AppFlow {



public interface IMessageEmitter
{
	void EmitCommand(ICommand command);
}



}
