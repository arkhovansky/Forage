namespace Lib.AppFlow {



public interface IMessage {}


public interface IInputEvent : IMessage {}


public interface ICommand : IMessage {}


public interface IDomainEvent : IMessage {}


public interface IPresentationEvent : IMessage {}



}
