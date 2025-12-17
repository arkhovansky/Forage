using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Lib.AppFlow.Internal;



namespace Lib.AppFlow {



public abstract class Context
	: IContext,
	  IContext_Internal,
	  IMessageEmitter
{
	protected readonly List<IContext> Children = new();

	protected readonly IMessageDispatcher MessageDispatcher;

	protected readonly Dictionary<Type, Delegate> Command_Handlers = new();

	//----------------------------------------------------------------------------------------------


	protected Context(IMessageDispatcher messageDispatcher)
	{
		MessageDispatcher = messageDispatcher;
	}


	//----------------------------------------------------------------------------------------------
	// IContext_Internal implementation


	IReadOnlyDictionary<Type, Delegate> IContext_Internal.Command_Handlers => Command_Handlers;

	public IController? Controller { get; protected set; }


	//----------------------------------------------------------------------------------------------
	// IContext implementation


	public IContext? Parent { get; set; }


	public virtual UniTask Start() { return UniTask.CompletedTask; }


	public virtual void Update()
	{
		DoUpdate();
		Children.ForEach(c => c.Update());
	}


	public virtual void LateUpdate()
	{
		DoLateUpdate();
		Children.ForEach(c => c.LateUpdate());
	}


	public virtual void Destroy() {}


	//----------------------------------------------------------------------------------------------
	// IMessageEmitter implementation


	public void Emit(IInputEvent evt)
	{
		Emit((IMessage) evt);
	}

	public void Emit(ICommand command)
	{
		Emit((IMessage) command);
	}

	public void Emit(IDomainEvent evt)
	{
		Emit((IMessage) evt);
	}

	public void Emit(IPresentationEvent evt)
	{
		Emit((IMessage) evt);
	}

	public virtual void Emit(IMessage message)
	{
		MessageDispatcher.Emit(message, this);
	}


	//----------------------------------------------------------------------------------------------
	// protected


	protected virtual void Add_Command_Handler<TMessage>(Action<TMessage> method)
		where TMessage : ICommand
	{
		Command_Handlers[typeof(TMessage)] = method;
	}

	protected virtual void Remove_Command_Handler<TMessage>()
		where TMessage : ICommand
	{
		Command_Handlers.Remove(typeof(TMessage));
	}


	protected virtual void AddChildContext(IContext child)
	{
		child.Parent = this;
		Children.Add(child);
	}


	protected virtual void DoUpdate()
	{
		UpdateController();
	}


	protected virtual void UpdateController()
	{
		Controller?.Update();
	}


	protected virtual void DoLateUpdate() {}
}



}
