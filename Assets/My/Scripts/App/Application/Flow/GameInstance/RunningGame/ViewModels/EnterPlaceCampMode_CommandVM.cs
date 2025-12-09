using System;

using Unity.Properties;

using Lib.UICore.Mvvm;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



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
