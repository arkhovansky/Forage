namespace App.Application.Framework.UICore.Gui {



public interface IGui
{
	IVisualNode RootVisualNode { get; }

	void SetRootVisualNode(IVisualNode visualNode);


	void AddView(IView view);

	void RemoveView(IView view);


	// IVisualNode GetVisualNode(IVisualNodePath visualNodePath);


	void SetVisualResource(IVisualNode visualNode, string resourceName);
}



}
