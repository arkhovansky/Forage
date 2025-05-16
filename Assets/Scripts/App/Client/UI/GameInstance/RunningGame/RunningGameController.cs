using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;


using Lib.Grid;
using Lib.VisualGrid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.HighLevel.Impl;
using App.Client.Framework.UICore.LowLevel;
using App.Client.Framework.UICore.Mvvm;

using App.Game.Ecs.Components.Singletons.HoveredTile;
using App.Game.Ecs.Components.Singletons.YearPeriod;
using App.Game.Meta;
using App.Services;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public class RunningGameController : Controller
{
	private readonly IGameInstance _game;

	private readonly HexLayout _hexLayout;

	private readonly ITerrainTypeRepository _terrainTypeRepository;
	private readonly IResourceTypeRepository _resourceTypeRepository;

	private readonly IGameService _gameService;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;
	private readonly ICommandRouter _commandRouter;

	private GameVM? _viewModel;
	private GameView? _uiView;

	private VisualHexGrid? _grid;

	private InputAction _pointAction;

	private AxialPosition? _hoveredTilePosition;



	public RunningGameController(IGameInstance game,
	                             HexLayout hexLayout,
	                             ITerrainTypeRepository terrainTypeRepository,
	                             IResourceTypeRepository resourceTypeRepository,
	                             IGameService gameService,
	                             IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_game = game;
		_hexLayout = hexLayout;

		_terrainTypeRepository = terrainTypeRepository;
		_resourceTypeRepository = resourceTypeRepository;

		_gameService = gameService;

		_gui = gui;
		_vvmBinder = vvmBinder;
		_commandRouter = commandRouter;

		base.AddCommandHandler<EndTurnCommand>(OnEndTurn);

		_pointAction = InputSystem.actions.FindAction("Point");
	}


	public override void Start()
	{
		_gameService.PopulateWorld(_game.Scene);

		_grid = new VisualHexGrid(_hexLayout, _game.Scene.Grid);

		_viewModel = new GameVM(this,
		                        _commandRouter,
		                        _terrainTypeRepository, _resourceTypeRepository);
		_uiView = new GameView(_viewModel,
		                       _gui, _vvmBinder);
		_gui.AddView(_uiView);
	}


	protected override void DoUpdate()
	{
		UpdateHoveredTile();
	}


	public override void UpdateViewModel()
	{
		_viewModel!.Update();
	}


	private void UpdateHoveredTile()
	{
		AxialPosition? tilePosition = GetHoveredTilePosition();

		if (tilePosition == _hoveredTilePosition)
			return;

		NotifySystems_HoveredTileChanged(tilePosition);
		_hoveredTilePosition = tilePosition;
	}


	private AxialPosition? GetHoveredTilePosition()
	{
		if (Camera.main == null)
			return null;

		var point = _pointAction.ReadValue<Vector2>();

		var mouseCameraRay = Camera.main.ScreenPointToRay(new Vector3(point.x, point.y, 0));
		var plane = new Plane(Vector3.up, Vector3.zero);

		if (!plane.Raycast(mouseCameraRay, out float enter))
			return null;

		Vector3 point3 = mouseCameraRay.GetPoint(enter);
		var gridPoint = new Vector2(point3.x, point3.z);

		return _grid!.GetAxialPosition(gridPoint);
	}


	private void NotifySystems_HoveredTileChanged(AxialPosition? tilePosition)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entityQuery = entityManager.CreateEntityQuery(typeof(CurrentYearPeriod));
		var singletonEntity = entityQuery.GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, new HoveredTileChanged_Event(tilePosition));
	}


	private void OnEndTurn(EndTurnCommand command)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entityQuery = entityManager.CreateEntityQuery(typeof(CurrentYearPeriod));
		var singletonEntity = entityQuery.GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, new AdvanceYearPeriod_Command());
	}
}



}
