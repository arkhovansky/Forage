using Cysharp.Threading.Tasks;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Gui;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Framework.UnityUICore.Flow;

using App.Application.Flow.GameInstance.RunningGame;
using App.Application.Settings;
using App.Game.Meta;



namespace App.Application.Flow {



public class ApplicationController : ApplicationController_Base
{
	private readonly IApplicationSettings _settings;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;
	private readonly ICommandRouter _commandRouter;

	private IGameInstance? _gameInstance;



	public ApplicationController(IApplicationSettings settings,
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

		var child = new RunningGameController(_gameInstance,
			_gui, _vvmBinder, _commandRouter);
		AddChildController(child);
		await child.Start();
	}
}



}
