using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

using Lib.Grid;
using Lib.VisualGrid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.HighLevel.Impl;
using App.Client.Framework.UICore.LowLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Game.Ecs.Components;
using App.Game.Ecs.Components.Singletons;
using App.Game.Ecs.Components.Singletons.YearPeriod;
using App.Game.Meta;
using App.Services;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public class RunningGameController : Controller
{
	private readonly IGameInstance _game;

	private readonly VisualRectangularHexMap3D _map;

	private readonly GameVM _viewModel;
	private readonly GameView _uiView;

	private readonly InputAction _pointAction;

	private AxialPosition? _hoveredTilePosition;

	private IUIMode _uiMode;

	private readonly DefaultUIMode _defaultUIMode;
	private readonly PlaceCampUIMode _placeCampUIMode;

	private bool _campPlaced;



	public RunningGameController(IGameInstance game,
	                             HexLayout3D hexLayout,
	                             ITerrainTypeRepository terrainTypeRepository,
	                             IResourceTypeRepository resourceTypeRepository,
	                             IBandMemberTypeRepository bandMemberTypeRepository,
	                             IGameService gameService,
	                             IGui gui, IVvmBinder vvmBinder, ICommandRouter commandRouter)
		: base(commandRouter)
	{
		_game = game;

		_map = new VisualRectangularHexMap3D(hexLayout, _game.Scene.Map);

		_viewModel = new GameVM(this,
			commandRouter,
			terrainTypeRepository, resourceTypeRepository, bandMemberTypeRepository);
		_uiView = new GameView(_viewModel,
			gui, vvmBinder);
		gui.AddView(_uiView);

		_pointAction = InputSystem.actions.FindAction("Point");

		base.AddCommandHandler<EndTurnCommand>(OnEndTurn);
		base.AddCommandHandler<EnterPlaceCampMode>(OnEnterPlaceCampMode);
		base.AddCommandHandler<PlaceCamp>(OnPlaceCamp);

		_defaultUIMode = new DefaultUIMode();
		_placeCampUIMode = new PlaceCampUIMode(commandRouter, this);

		_uiMode = _defaultUIMode;

		_viewModel.EnterPlaceCampModeCommand.IsVisible = true;
		_viewModel.EndTurnCommand.IsVisible = false;


		gameService.PopulateWorld(_game.Scene);
	}


	public override void Start()
	{
	}


	protected override void DoUpdate()
	{
		var newHoveredTilePosition = GetHoveredTilePosition();

		_uiMode.Update(_hoveredTilePosition, newHoveredTilePosition);

		_hoveredTilePosition = newHoveredTilePosition;
	}


	public override void UpdateViewModel()
	{
		_viewModel.Update();
	}


	#region Command handlers

	private void OnEndTurn(EndTurnCommand command)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entityQuery = entityManager.CreateEntityQuery(typeof(CurrentYearPeriod));
		var singletonEntity = entityQuery.GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, new AdvanceYearPeriod_Command());
	}


	private void OnEnterPlaceCampMode(EnterPlaceCampMode command)
	{
		if (_campPlaced)
			return;
		if (_uiMode == _placeCampUIMode)
			return;

		_uiMode = _placeCampUIMode;
	}


	private void OnPlaceCamp(PlaceCamp command)
	{
		PlaceCamp(command.Position);

		_campPlaced = true;

		_uiMode = _defaultUIMode;

		_viewModel.EnterPlaceCampModeCommand.IsVisible = false;
		_viewModel.EndTurnCommand.IsVisible = true;
	}

	#endregion


	private AxialPosition? GetHoveredTilePosition()
	{
		if (Camera.main == null)
			return null;

		var point = _pointAction.ReadValue<Vector2>();

		var mouseCameraRay = Camera.main.ScreenPointToRay(new Vector3(point.x, point.y, 0));

		return _map.GetAxialPosition(mouseCameraRay);
	}


	private void PlaceCamp(AxialPosition position)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var prefabReferences = entityManager.CreateEntityQuery(typeof(PrefabReferences))
			.GetSingleton<PrefabReferences>();

		var campEntity = entityManager.Instantiate(prefabReferences.Camp);

		entityManager.SetComponentData(campEntity, new TilePosition(position));

		var inTilePosition = new Vector2(0f, (_map.Layout.CellSize.y / 2) * -0.75f);
		entityManager.SetComponentData(campEntity,
			_map.Layout.GetCellLocalTransform(position)
				.Translate(new float3(inTilePosition.x, 0.01f, inTilePosition.y))
				.ApplyScale(0.25f));
	}
}



}
