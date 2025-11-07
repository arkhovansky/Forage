using Cysharp.Threading.Tasks;

using Lib.VisualGrid;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Gui;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Framework.UnityUICore.Flow;

using App.Application.Flow.GameInstance.RunningGame;
using App.Application.Services;
using App.Game.Database;
using App.Game.Meta;



namespace App.Application.Flow {



public class ApplicationController : ApplicationController_Base
{
	private readonly HexLayout3D _hexLayout;

	private readonly ITerrainTypeRepository _terrainTypeRepository;
	private readonly IResourceTypeRepository _resourceTypeRepository;
	private readonly IBandMemberTypeRepository _bandMemberTypeRepository;

	private readonly IRunningGameInitializer _runningGameInitializer;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;
	private readonly ICommandRouter _commandRouter;

	private IGameInstance? _gameInstance;



	public ApplicationController(HexLayout3D hexLayout,
	                             ITerrainTypeRepository terrainTypeRepository,
	                             IResourceTypeRepository resourceTypeRepository,
	                             IBandMemberTypeRepository bandMemberTypeRepository,
	                             IRunningGameInitializer runningGameInitializer,
	                             IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_hexLayout = hexLayout;

		_terrainTypeRepository = terrainTypeRepository;
		_resourceTypeRepository = resourceTypeRepository;
		_bandMemberTypeRepository = bandMemberTypeRepository;

		_runningGameInitializer = runningGameInitializer;

		_gui = gui;
		_vvmBinder = vvmBinder;
		_commandRouter = commandRouter;
	}


	public override async UniTask Start()
	{
		_gameInstance = new GameInstance_Stub();

		var child = new RunningGameController(_gameInstance,
			_hexLayout,
			_terrainTypeRepository, _resourceTypeRepository, _bandMemberTypeRepository,
			_runningGameInitializer,
			_gui, _vvmBinder, _commandRouter);
		AddChildController(child);
		await child.Start();
	}
}



}
