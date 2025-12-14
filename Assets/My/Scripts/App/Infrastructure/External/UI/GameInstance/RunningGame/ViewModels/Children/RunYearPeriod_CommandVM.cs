using System;

using Unity.Properties;

using Lib.UICore.Mvvm;



namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels.Children {



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
