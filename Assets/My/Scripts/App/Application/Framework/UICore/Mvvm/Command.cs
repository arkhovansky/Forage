using System;



namespace App.Application.Framework.UICore.Mvvm {



public class Command : ICommand
{
	private readonly Action _action;


	public Command(Action action)
	{
		_action = action;
	}


	public void Execute()
	{
		_action();
	}
}



}
