using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;



namespace Lib.AppFlow {



public abstract class Controller : IController
{
	public IController? Parent { get; set; }

	public IReadOnlyDictionary<Type, Delegate> CommandHandlers => commandHandlers;


	protected List<IController> Children = new();

	protected readonly ICommandRouter CommandRouter;

	protected readonly Dictionary<Type, Delegate> commandHandlers = new();



	protected Controller(ICommandRouter commandRouter)
	{
		CommandRouter = commandRouter;

		Parent = null;
	}



	public virtual UniTask Start() { return UniTask.CompletedTask; }


	public virtual void Update()
	{
		DoUpdate();
		Children.ForEach(c => c.Update());
	}


	protected virtual void DoUpdate() {}


	public virtual void UpdateViewModels()
	{
		UpdateViewModel();
		Children.ForEach(c => c.UpdateViewModels());
	}


	public virtual void UpdateViewModel() {}


	public virtual void Destroy() {}



	protected virtual void AddCommandHandler<TCommand>(Action<TCommand> method)
	{
		commandHandlers[typeof(TCommand)] = method;
	}

	protected virtual void RemoveCommandHandler<TCommand>()
	{
		commandHandlers.Remove(typeof(TCommand));
	}


	protected virtual void EmitCommand(ICommand command)
	{
		CommandRouter.EmitCommand(command, this);
	}


	protected virtual void AddChildController(IController child)
	{
		child.Parent = this;
		Children.Add(child);

		CommandRouter.AddController(child);
	}
}



}
