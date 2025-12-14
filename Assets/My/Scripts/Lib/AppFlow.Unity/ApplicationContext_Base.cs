using UnityEngine;



namespace Lib.AppFlow.Unity {



public abstract class ApplicationContext_Base : Context
{
	protected ApplicationContext_Base(
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
