using UnityEngine;

using Lib.AppFlow.Impl;



namespace Lib.AppFlow.Unity {



public abstract class ApplicationController_Base : Controller
{
	protected ApplicationController_Base(
		ICommandRouter commandRouter
	)
		: base(commandRouter)
	{
		base.AddCommandHandler<ExitApplicationCommand>(OnQuitCommand);

		// Intercept standard OS quit commands
		Application.wantsToQuit += OnWantsToQuit;
	}



	protected virtual void OnQuitCommand(ExitApplicationCommand command)
	{
		Application.wantsToQuit -= OnWantsToQuit;
		Application.Quit();
	}



	protected virtual bool OnWantsToQuit()
	{
		EmitCommand(new ExitApplicationCommand());

		return false;
	}
}



}
