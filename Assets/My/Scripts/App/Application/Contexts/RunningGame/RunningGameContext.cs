using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Contexts.RunningGame.Services;
using App.Game.Meta;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Application.Contexts.RunningGame {



public partial class RunningGameContext : Context
{
	private readonly IGameInstance _game;

	private readonly IEcsSystems_Service _ecsSystems_Service;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;


	private IInGameMode _inGameMode = null!;

	private ILoopComponent _runningGame = null!;

	private ILoopComponent _uiModel = null!;

	private ILoopComponent _sceneController = null!;

	private ILoopComponent _screenUI_VM = null!;
	private Lib.UICore.Gui.IView _screenUI_View = null!;

	//----------------------------------------------------------------------------------------------


	public RunningGameContext(IGameInstance game,
	                          IEcsSystems_Service ecsSystems_Service,
	                          IGui gui, IVvmBinder vvmBinder, IMessageDispatcher messageDispatcher)
		: base(messageDispatcher)
	{
		_game = game;

		_ecsSystems_Service = ecsSystems_Service;

		_gui = gui;
		_vvmBinder = vvmBinder;
	}


	public override async UniTask Start()
	{
		_inGameMode = Create_InGameMode();

		await _inGameMode.Enter();
		// GameDatabase.Instance is available now

		Compose(out var localeFactory,
		        out var runningGameInitializer,
		        out var controller,
		        out var worldUI_View);

		var locale = localeFactory.Create(_game.LocaleId);

		runningGameInitializer.Initialize(locale);
		_runningGame.Start();

		Controller = controller;
		Controller.Start();

		var sceneViewController = Create_SceneViewController(locale.Map);
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
