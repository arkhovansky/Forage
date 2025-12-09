using UnityEngine;
using UnityEngine.UIElements;

using Lib.UICore.Gui;



namespace Lib.UICore.Unity.Gui {



public class Gui : Lib.UICore.Gui.Impl.Gui
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
