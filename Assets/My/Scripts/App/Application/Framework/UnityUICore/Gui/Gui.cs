using UnityEngine;
using UnityEngine.UIElements;

using App.Application.Framework.UICore.Gui;



namespace App.Application.Framework.UnityUICore.Gui {



public class Gui : App.Application.Framework.UICore.Gui.Impl.Gui
{
	public override void SetVisualResource(IVisualNode visualNode, string resourceName)
	{
		var asset = Resources.Load<VisualTreeAsset>(resourceName);

		var element = (visualNode as UITKVisualNode)!.Element;

		element.Clear();
		asset.CloneTree(element);
	}
}



}
