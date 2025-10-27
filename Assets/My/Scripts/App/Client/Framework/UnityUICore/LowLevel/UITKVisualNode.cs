using App.Client.Framework.UICore.LowLevel;
using UnityEngine.UIElements;

namespace App.Client.Framework.UnityUICore.LowLevel
{
	public class UITKVisualNode : IVisualNode
	{
		public VisualElement Element { get; }



		public UITKVisualNode(VisualElement element)
		{
			Element = element;
		}
	}
}
