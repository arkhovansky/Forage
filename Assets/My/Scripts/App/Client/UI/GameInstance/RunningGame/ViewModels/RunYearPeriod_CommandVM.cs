using System;

using Unity.Properties;

using App.Client.Framework.UICore.Mvvm;



namespace App.Client.UI.GameInstance.RunningGame.ViewModels {



public class RunYearPeriod_CommandVM : Command
{
	[CreateProperty]
	public bool IsVisible { get; set; }


	public RunYearPeriod_CommandVM(Action action)
		: base(action)
	{
	}
}



}
