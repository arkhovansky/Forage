using System;



namespace App.Client.Framework.UICore.LowLevel.Impl {



public abstract class Gui : IGui
{
	public IVisualNode RootVisualNode {
		get {
			if (rootVisualNode == null) throw new InvalidOperationException("Root visual node is null");

			return rootVisualNode;
		}
		private set => rootVisualNode = value;
	}


	protected IView? LastView;

	protected IVisualNode? rootVisualNode;



	public void SetRootVisualNode(IVisualNode visualNode)
	{
		RootVisualNode = visualNode;

		LastView?.Build();
	}



	public void AddView(IView view)
	{
		//TODO

		LastView = view;
		if (rootVisualNode != null)
			view.Build();
	}

	public void RemoveView(IView view)
	{
		//TODO

		LastView = null;
	}



	// public abstract IVisualNode GetVisualNode(IVisualNodePath visualNodePath);

	public abstract void SetVisualResource(IVisualNode visualNode, string resourceName);
}



}
