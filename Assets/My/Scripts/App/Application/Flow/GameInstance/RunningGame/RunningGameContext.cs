using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Services;
using App.Game.Meta;



namespace App.Application.Flow.GameInstance.RunningGame {



public partial class RunningGameContext : Context
{
	private readonly IGameInstance _game;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;


	private IInGameMode _inGameMode = null!;

	private ILoopComponent _runningGame = null!;

	private ILoopComponent _sceneController = null!;

	private ILoopComponent _uiVM = null!;
	private IView _uiView = null!;

	//----------------------------------------------------------------------------------------------


	public RunningGameContext(IGameInstance game,
	                          IGui gui, IVvmBinder vvmBinder, IMessageDispatcher messageDispatcher)
		: base(messageDispatcher)
	{
		_game = game;

		_gui = gui;
		_vvmBinder = vvmBinder;
	}


	public override async UniTask Start()
	{
		_inGameMode = Create_InGameMode();

		await _inGameMode.Enter();
		// GameDatabase.Instance is available now

		Compose(out var localeFactory,
		        out var runningGameInitializer);

		var locale = localeFactory.Create(_game.LocaleId);

		runningGameInitializer.Initialize(locale);
		_runningGame.Start();

		Controller = Create_Controller(locale.Map);
		Controller.Start();
	}


	protected override void DoUpdate()
	{
		// Controller should be updated first to handle Model-originating state changes in previous frame's LateUpdate()
		UpdateController();
		_sceneController.Update();
	}



	protected override void DoLateUpdate()
	{
		_uiVM.LateUpdate();
	}
}



}
