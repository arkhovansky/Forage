using System;
using System.Collections.Generic;



namespace App.Client.Framework.UICore.HighLevel.Impl {



public abstract class Controller : IController
{
	public IController? Parent { get; set; }

	public IReadOnlyDictionary<Type, Delegate> CommandHandlers => commandHandlers;

	// public IVisualNode? VisualNode { get; set; }


	protected List<IController> Children = new();

	protected readonly ICommandRouter CommandRouter;

	protected readonly Dictionary<Type, Delegate> commandHandlers = new();



	protected Controller(ICommandRouter commandRouter)
	{
		CommandRouter = commandRouter;

		Parent = null;
	}



	public virtual void Start() {}


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


	// protected virtual void ReplaceChildController(IController oldChild, IController newChild)
	// {
	// 	CommandRouter.RemoveController(oldChild);
	// 	oldChild.Destroy();
	//
	// 	AddChildController(newChild);
	// }
}



}
