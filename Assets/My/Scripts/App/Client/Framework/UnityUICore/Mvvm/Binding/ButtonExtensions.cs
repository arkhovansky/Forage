using UnityEngine.UIElements;

using App.Client.Framework.UICore.Mvvm;



namespace App.Client.Framework.UnityUICore.Mvvm.Binding {



public static class ButtonExtensions
{
	public static void BindCommand(this Button button, ICommand command)
	{
		// command.Enabled.ListenWeak(button.SetEnabled);

		button.RegisterCallback<ClickEvent>(_ => command.Execute());
	}
}



}
