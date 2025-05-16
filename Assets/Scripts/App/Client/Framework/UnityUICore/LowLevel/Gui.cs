using UnityEngine;
using UnityEngine.UIElements;

using App.Client.Framework.UICore.LowLevel;



namespace App.Client.Framework.UnityUICore.LowLevel {



public class Gui : App.Client.Framework.UICore.LowLevel.Impl.Gui
{
	// public override IVisualNode GetVisualNode(IVisualNodePath visualNodePath)
	// {
	// 	//TODO
	//
	// 	return RootVisualNode;
	// }


	public override void SetVisualResource(IVisualNode visualNode, string resourceName)
	{
		var asset = Resources.Load<VisualTreeAsset>(resourceName);

		var element = (visualNode as UITKVisualNode)!.Element;

		element.Clear();
		asset.CloneTree(element);
	}
}



}
