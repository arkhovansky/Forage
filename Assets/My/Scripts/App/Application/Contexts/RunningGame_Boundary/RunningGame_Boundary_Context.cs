using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.AppFlow.Resolution;

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
	private readonly IContextHost _contextHost;

	//----------------------------------------------------------------------------------------------


	public RunningGame_Boundary_Context(IGameInstance game,
	                                    IInGameMode inGameMode,
	                                    ILoadedContextComposer loadedContextComposer,
	                                    IContextHost contextHost,
	                                    IMessageDispatcher messageDispatcher)
		: base(messageDispatcher)
	{
		_game = game;
		_inGameMode = inGameMode;
		_loadedContextComposer = loadedContextComposer;
		_contextHost = contextHost;
	}


	public override async UniTask Start()
	{
		await _inGameMode.Enter();
		// GameDatabase.Instance is available now

		_loadedContextComposer.Compose(
			out var localeFactory,
			out var runningGameInitializer,
			out var runningGame);

		var locale = localeFactory.Create(_game.LocaleId);

		runningGameInitializer.Initialize(locale);

		var childRequest = _contextHost.New_ContextRequest()
			.Subject(runningGame)
			.Build();
		var child = _contextHost.CreateContext(childRequest, this);
		AddChildContext(child);
		await child.Start();
	}
}



}
