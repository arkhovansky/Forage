using Cysharp.Threading.Tasks;

using Lib.AppFlow;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database;
using App.Application.Contexts.RunningGame_Boundary.Composition;
using App.Application.Contexts.RunningGame_Boundary.Services;
using App.Game.Meta;



namespace App.Application.Contexts.RunningGame_Boundary {



/// <summary>
/// Handles resource loading and running game initialization; creates RunningGameInstance
/// </summary>
public class RunningGame_Boundary_Context : Context
{
	private readonly IGameInstance _game;

	private readonly IInGameMode _inGameMode;
	private readonly ILoadedContextComposer _loadedContextComposer;

	//----------------------------------------------------------------------------------------------


	public RunningGame_Boundary_Context(IGameInstance game,
	                                    IInGameMode inGameMode,
	                                    ILoadedContextComposer loadedContextComposer)
	{
		_game = game;
		_inGameMode = inGameMode;
		_loadedContextComposer = loadedContextComposer;
	}


	public override async UniTask Start()
	{
		await _inGameMode.Enter();
		// GameDatabase.Instance is available now

		_loadedContextComposer.Compose(
			GameDatabase.Instance,
			out var localeFactory,
			out var runningGameInitializer,
			out var runningGame);

		var locale = localeFactory.Create(_game.Setup.LocaleId);

		runningGameInitializer.Initialize(locale);

		var childRequest = ContextHost.New_ContextRequest()
			.Subject(runningGame)
			.Build();
		var child = ContextHost.CreateContext(childRequest, this);
		AddChildContext(child);
		await child.Start();
	}
}



}
