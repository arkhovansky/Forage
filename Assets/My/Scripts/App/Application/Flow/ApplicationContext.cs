using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.AppFlow.Unity;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Flow.GameInstance.RunningGame;
using App.Application.Settings;
using App.Game.Meta;



namespace App.Application.Flow {



public class ApplicationContext : ApplicationContext_Base
{
	private readonly IApplicationSettings _settings;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;
	private readonly ICommandRouter _commandRouter;

	private IGameInstance? _gameInstance;



	public ApplicationContext(IApplicationSettings settings,
	                          IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_settings = settings;
		_gui = gui;
		_vvmBinder = vvmBinder;
		_commandRouter = commandRouter;
	}


	public override async UniTask Start()
	{
		var localeId = _settings.DefaultLocale;
		_gameInstance = new Game.Meta.Impl.GameInstance(localeId);

		var child = new RunningGameContext(_gameInstance,
			_gui, _vvmBinder, _commandRouter);
		AddChildContext(child);
		await child.Start();
	}
}



}
