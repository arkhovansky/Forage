namespace Lib.AppFlow {



public interface IInputEvent_Emitter
{
	void Emit(IInputEvent evt);
}


public interface ICommand_Emitter
{
	void Emit(ICommand command);
}


public interface IDomainEvent_Emitter
{
	void Emit(IDomainEvent evt);
}


public interface IPresentationEvent_Emitter
{
	void Emit(IPresentationEvent evt);
}


public interface IMessageEmitter
	: IInputEvent_Emitter,
	  ICommand_Emitter,
	  IDomainEvent_Emitter,
	  IPresentationEvent_Emitter
{
	void Emit(IMessage message);
}



}
