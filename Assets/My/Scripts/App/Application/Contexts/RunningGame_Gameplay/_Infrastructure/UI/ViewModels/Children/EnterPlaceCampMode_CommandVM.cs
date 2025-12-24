using System;

using Unity.Properties;

using Lib.UICore.Mvvm;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ViewModels.Children {



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
