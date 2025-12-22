using Cysharp.Threading.Tasks;

using Lib.AppFlow;

using App.Application.Contexts.RunningGame.Composition;
using App.Application.Contexts.RunningGame.Services;
using App.Game.Meta;



namespace App.Application.Contexts.RunningGame {



public class RunningGameContext : Context
{
	private readonly IGameInstance _game;

	private readonly IInGameMode _inGameMode;
	private readonly ILoadedContextComposer _loadedContextComposer;

	private ILoopComponent _runningGame = null!;

	private ILoopComponent _uiModel = null!;

	private ILoopComponent _sceneController = null!;

	private ILoopComponent _screenUI_VM = null!;
	private Lib.UICore.Gui.IView _screenUI_View = null!;

	//----------------------------------------------------------------------------------------------


	public RunningGameContext(IGameInstance game,
	                          IMessageDispatcher messageDispatcher,
	                          IInGameMode inGameMode,
	                          ILoadedContextComposer loadedContextComposer)
		: base(messageDispatcher)
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
			this,
			out var localeFactory,
			out var runningGameInitializer,
			out _runningGame,
			out _uiModel,
			out var controller,
			out var worldUI_View,
			out _screenUI_VM,
			out _screenUI_View);

		var locale = localeFactory.Create(_game.LocaleId);

		runningGameInitializer.Initialize(locale);
		_runningGame.Start();

		Controller = controller;
		Controller.Start();

		var sceneViewController = _loadedContextComposer.Create_SceneViewController(locale.Map, this);
		_sceneController = sceneViewController;

		AddView(sceneViewController);
		AddView(worldUI_View);

		_uiModel.Start();
	}


	protected override void DoUpdate()
	{
		// Controller should be updated first to handle Model-originating state changes in previous frame's LateUpdate()
		UpdateController();
		_sceneController.Update();
	}



	protected override void DoLateUpdate()
	{
		// Update model first because it can change UI mode
		_runningGame.LateUpdate();
		_screenUI_VM.LateUpdate();
	}
}



}
