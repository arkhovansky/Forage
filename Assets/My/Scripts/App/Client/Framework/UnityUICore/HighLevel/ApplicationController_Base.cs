using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.HighLevel.Impl;



namespace App.Client.Framework.UnityUICore.HighLevel {



public abstract class ApplicationController_Base : Controller
{
	protected ApplicationController_Base(
		ICommandRouter commandRouter
	)
		: base(commandRouter)
	{
		base.AddCommandHandler<ExitApplicationCommand>(OnQuitCommand);

		// Intercept standard OS quit commands
		UnityEngine.Application.wantsToQuit += OnWantsToQuit;
	}



	protected virtual void OnQuitCommand(ExitApplicationCommand command)
	{
		UnityEngine.Application.wantsToQuit -= OnWantsToQuit;
		UnityEngine.Application.Quit();
	}



	protected virtual bool OnWantsToQuit()
	{
		EmitCommand(new ExitApplicationCommand());

		return false;
	}
}



}
