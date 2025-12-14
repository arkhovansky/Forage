using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;



namespace Lib.AppFlow {



public abstract class Context : IContext, IMessageEmitter
{
	public IContext? Parent { get; set; }

	public IReadOnlyDictionary<Type, Delegate> CommandHandlers => commandHandlers;

	public IController? Controller { get; protected set; }


	protected readonly List<IContext> Children = new();

	protected readonly ICommandRouter CommandRouter;

	// ReSharper disable once InconsistentNaming
	protected readonly Dictionary<Type, Delegate> commandHandlers = new();

	//----------------------------------------------------------------------------------------------


	protected Context(ICommandRouter commandRouter)
	{
		CommandRouter = commandRouter;
	}


	//----------------------------------------------------------------------------------------------
	// IContext implementation


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
		CommandRouter.EmitCommand(command, this);
	}


	//----------------------------------------------------------------------------------------------
	// protected


	protected virtual void AddCommandHandler<TCommand>(Action<TCommand> method)
	{
		commandHandlers[typeof(TCommand)] = method;
	}

	protected virtual void RemoveCommandHandler<TCommand>()
	{
		commandHandlers.Remove(typeof(TCommand));
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
