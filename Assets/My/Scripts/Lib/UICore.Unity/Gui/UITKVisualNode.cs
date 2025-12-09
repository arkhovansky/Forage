using UnityEngine.UIElements;

using Lib.UICore.Gui;



namespace Lib.UICore.Unity.Gui {



public class UITKVisualNode : IVisualNode
{
	public VisualElement Element { get; }



	public UITKVisualNode(VisualElement element)
	{
		Element = element;
	}
}



}
