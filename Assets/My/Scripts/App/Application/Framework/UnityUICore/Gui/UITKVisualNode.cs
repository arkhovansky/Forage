using UnityEngine.UIElements;

using App.Application.Framework.UICore.Gui;



namespace App.Application.Framework.UnityUICore.Gui {



public class UITKVisualNode : IVisualNode
{
	public VisualElement Element { get; }



	public UITKVisualNode(VisualElement element)
	{
		Element = element;
	}
}



}
