using App.Application.Contexts.RunningGame_Boundary.Services;
using App.Game.Core;



namespace App.Application.Contexts.RunningGame_Boundary.Composition {



public interface ILoadedContextComposer
{
	void Compose(out ILocaleFactory localeFactory,
	             out IRunningGameInitializer runningGameInitializer,
	             out IRunningGameInstance runningGame);
}



}
