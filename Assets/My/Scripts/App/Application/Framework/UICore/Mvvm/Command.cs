using System;

// using Sodium.Frp;



namespace App.Application.Framework.UICore.Mvvm {



public class Command : ICommand
{
	// public Cell<bool> Enabled { get; }


	private readonly Action _action;


	public Command(Action action)
	{
		_action = action;
	}


	public void Execute()
	{
		_action();
	}



	// protected Command(Cell<bool> enabled)
	// {
	// 	Enabled = enabled;
	// }
}



}
