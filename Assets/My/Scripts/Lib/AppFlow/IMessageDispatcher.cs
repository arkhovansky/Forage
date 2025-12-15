namespace Lib.AppFlow {



public interface IMessageDispatcher
{
	void Emit(IMessage message, IContext emitter);

	void Update();
}



}
