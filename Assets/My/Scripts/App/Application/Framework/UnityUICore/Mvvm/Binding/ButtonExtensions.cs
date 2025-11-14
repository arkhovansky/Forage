using UnityEngine.UIElements;

using App.Application.Framework.UICore.Mvvm;



namespace App.Application.Framework.UnityUICore.Mvvm.Binding {



public static class ButtonExtensions
{
	public static void BindCommand(this Button button, ICommand command)
	{
		button.RegisterCallback<ClickEvent>(_ => command.Execute());
	}
}



}
