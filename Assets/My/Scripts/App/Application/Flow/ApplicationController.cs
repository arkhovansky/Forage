using Cysharp.Threading.Tasks;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Gui;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Framework.UnityUICore.Flow;

using App.Application.Flow.GameInstance.RunningGame;
using App.Game.Meta;



namespace App.Application.Flow {



public class ApplicationController : ApplicationController_Base
{
	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;
	private readonly ICommandRouter _commandRouter;

	private IGameInstance? _gameInstance;



	public ApplicationController(IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_gui = gui;
		_vvmBinder = vvmBinder;
		_commandRouter = commandRouter;
	}


	public override async UniTask Start()
	{
		_gameInstance = new GameInstance_Stub();

		var child = new RunningGameController(_gameInstance,
			_gui, _vvmBinder, _commandRouter);
		AddChildController(child);
		await child.Start();
	}
}



}
