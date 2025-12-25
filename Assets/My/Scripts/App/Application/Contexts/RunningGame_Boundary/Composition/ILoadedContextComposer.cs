using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database;
using App.Application.Contexts.RunningGame_Boundary.Services;
using App.Game.Core;



namespace App.Application.Contexts.RunningGame_Boundary.Composition {



public interface ILoadedContextComposer
{
	void Compose(
		GameDatabase database,
		out ILocaleFactory localeFactory,
		out IRunningGameInitializer runningGameInitializer,
		out IRunningGameInstance runningGame);
}



}
