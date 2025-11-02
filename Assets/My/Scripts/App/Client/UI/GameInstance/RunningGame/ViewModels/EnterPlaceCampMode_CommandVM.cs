using System;

using Unity.Properties;

using App.Client.Framework.UICore.Mvvm;



namespace App.Client.UI.GameInstance.RunningGame.ViewModels {



public class EnterPlaceCampMode_CommandVM : Command
{
	[CreateProperty]
	public bool IsVisible { get; set; }


	public EnterPlaceCampMode_CommandVM(Action action)
		: base(action)
	{
	}
}



}
