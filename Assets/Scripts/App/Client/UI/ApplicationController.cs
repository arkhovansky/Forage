using Lib.VisualGrid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.LowLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Client.Framework.UnityUICore.HighLevel;

using App.Client.UI.GameInstance.RunningGame;
using App.Game.Meta;
using App.Services;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI {



public class ApplicationController : ApplicationController_Base
{
	private readonly HexLayout3D _hexLayout;

	private readonly ITerrainTypeRepository _terrainTypeRepository;
	private readonly IResourceTypeRepository _resourceTypeRepository;
	private readonly IBandMemberTypeRepository _bandMemberTypeRepository;

	private readonly IGameService _gameService;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;
	private readonly ICommandRouter _commandRouter;

	private IGameInstance? _gameInstance;



	public ApplicationController(HexLayout3D hexLayout,
	                             ITerrainTypeRepository terrainTypeRepository,
	                             IResourceTypeRepository resourceTypeRepository,
	                             IBandMemberTypeRepository bandMemberTypeRepository,
	                             IGameService gameService,
	                             IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_hexLayout = hexLayout;

		_terrainTypeRepository = terrainTypeRepository;
		_resourceTypeRepository = resourceTypeRepository;
		_bandMemberTypeRepository = bandMemberTypeRepository;

		_gameService = gameService;

		_gui = gui;
		_vvmBinder = vvmBinder;
		_commandRouter = commandRouter;
	}


	public override void Start()
	{
		_gameInstance = new Game.Meta.GameInstance();

		var child = new RunningGameController(_gameInstance,
			_hexLayout,
			_terrainTypeRepository, _resourceTypeRepository, _bandMemberTypeRepository,
			_gameService,
			_gui, _vvmBinder, _commandRouter);
		AddChildController(child);
		child.Start();
	}
}



}
