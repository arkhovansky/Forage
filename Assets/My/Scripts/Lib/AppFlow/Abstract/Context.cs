using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Lib.AppFlow.Internal;
using Lib.AppFlow.Resolution;



namespace Lib.AppFlow {



public abstract class Context
	: IContext,
	  IContext_Internal,
	  IMessageEmitter
{
	protected readonly List<IContext> Children = new();

	protected readonly Dictionary<Type, Delegate> Command_Handlers = new();

	protected readonly List<IView> Views = new();


	protected IMessageDispatcher MessageDispatcher { get; private set; } = null!;

	protected IContextHost ContextHost { get; private set; } = null!;


	private IContextData _contextData = null!;


	//----------------------------------------------------------------------------------------------
	// IContext_Internal implementation


	void IContext_Internal.Init(IMessageDispatcher messageDispatcher,
	                            IContextData contextData,
	                            IContextHost contextHost)
	{
		MessageDispatcher = messageDispatcher;
		_contextData = contextData;
		ContextHost = contextHost;
	}


	IReadOnlyDictionary<Type, Delegate> IContext_Internal.Command_Handlers => Command_Handlers;

	public IController? Controller { get; protected set; }

	IReadOnlyList<IView> IContext_Internal.Views => Views;

	IContextData IContext_Internal.ContextData => _contextData;


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


	protected virtual void AddView(IView view)
	{
		Views.Add(view);
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
