using System;

using Unity.Properties;

using App.Client.Framework.UICore.Mvvm;



namespace App.Client.UI.GameInstance.RunningGame {



public class EndTurn_CommandVM : Command
{
	[CreateProperty]
	public bool IsVisible { get; set; }


	public EndTurn_CommandVM(Action action)
		: base(action)
	{
	}
}



}
