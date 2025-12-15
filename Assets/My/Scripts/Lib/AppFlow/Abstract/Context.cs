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

	protected readonly ICommandDispatcher CommandDispatcher;

	protected readonly Dictionary<Type, Delegate> CommandHandlers = new();

	//----------------------------------------------------------------------------------------------


	protected Context(ICommandDispatcher commandDispatcher)
	{
		CommandDispatcher = commandDispatcher;
	}


	//----------------------------------------------------------------------------------------------
	// IContext_Internal implementation


	IReadOnlyDictionary<Type, Delegate> IContext_Internal.CommandHandlers => CommandHandlers;

	public IController? Controller { get; protected set; }


	//----------------------------------------------------------------------------------------------
	// IContext implementation


	public IContext? Parent { get; set; }


	public virtual UniTask Start() { return UniTask.CompletedTask; }


	public virtual void Update()
	{
		UpdateController();
		Children.ForEach(c => c.Update());
	}


	public virtual void UpdateViewModels()
	{
		UpdateViewModel();
		Children.ForEach(c => c.UpdateViewModels());
	}


	public virtual void Destroy() {}


	//----------------------------------------------------------------------------------------------
	// IMessageEmitter implementation


	public virtual void EmitCommand(ICommand command)
	{
		CommandDispatcher.EmitCommand(command, this);
	}


	//----------------------------------------------------------------------------------------------
	// protected


	protected virtual void AddCommandHandler<TCommand>(Action<TCommand> method)
	{
		CommandHandlers[typeof(TCommand)] = method;
	}

	protected virtual void RemoveCommandHandler<TCommand>()
	{
		CommandHandlers.Remove(typeof(TCommand));
	}


	protected virtual void AddChildContext(IContext child)
	{
		child.Parent = this;
		Children.Add(child);
	}


	protected virtual void UpdateController()
	{
		Controller?.Update();
	}


	protected virtual void UpdateViewModel() {}
}



}
