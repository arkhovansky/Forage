using UnityEngine.UIElements;

using Lib.UICore.Mvvm;



namespace Lib.UICore.Unity.Mvvm.Binding {



public static class ButtonExtensions
{
	public static void BindCommand(this Button button, ICommand command)
	{
		button.RegisterCallback<ClickEvent>(_ => command.Execute());
	}
}



}
