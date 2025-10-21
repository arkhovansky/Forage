using UnityEngine;

using Lib.VisualGrid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.HighLevel.Impl;
using App.Client.Framework.UICore.LowLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Game.Meta;
using App.Services;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public partial class RunningGameController : Controller
{
	private readonly IGameInstance _game;

	private readonly VisualRectangularHexMap3D _map;

	private readonly GameVM _viewModel;
	private readonly GameView _uiView;

	private IUIMode _uiMode;

	private readonly DefaultUIMode _defaultUIMode;
	private readonly PlaceCampUIMode _placeCampUIMode;

	private bool _campPlaced;

	//----------------------------------------------------------------------------------------------


	private interface IUIMode
	{
		void OnEnter() {}
		void OnExit() {}
	}

	public class DefaultUIMode : IUIMode {}

	//----------------------------------------------------------------------------------------------


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

		_map = new VisualRectangularHexMap3D(_game.Scene.Map, hexLayout);

		_viewModel = new GameVM(this,
			commandRouter,
			terrainTypeRepository, resourceTypeRepository, bandMemberTypeRepository);
		_uiView = new GameView(_viewModel,
			gui, vvmBinder);
		gui.AddView(_uiView);

		base.AddCommandHandler<HoveredTileChanged>(OnHoveredTileChanged);
		base.AddCommandHandler<EndTurnCommand>(OnEndTurn);
		base.AddCommandHandler<EnterPlaceCampMode>(OnEnterPlaceCampMode);
		base.AddCommandHandler<PlaceCamp>(OnPlaceCamp);

		_defaultUIMode = new DefaultUIMode();
		_placeCampUIMode = new PlaceCampUIMode(this);

		_uiMode = _defaultUIMode;


		gameService.PopulateWorld(_game.Scene);
	}


	public override void Start()
	{
		var sceneViewController = new SceneViewController(Camera.main!, _map,
		                                                  CommandRouter);
		AddChildController(sceneViewController);
		sceneViewController.Start();


		_uiMode.OnEnter();

		_viewModel.EnterPlaceCampModeCommand.IsVisible = true;
		_viewModel.EndTurnCommand.IsVisible = false;
	}


	public override void UpdateViewModel()
	{
		_viewModel.Update();
	}


	#region Command handlers

	private void OnEndTurn(EndTurnCommand command)
	{
		EcsService.SendEcsCommand(new PlayYearPeriod());
	}


	private void OnEnterPlaceCampMode(EnterPlaceCampMode command)
	{
		if (_campPlaced)
			return;

		SetUIMode(_placeCampUIMode);
	}


	private void OnPlaceCamp(PlaceCamp command)
	{
		EcsService.SendEcsCommand(new App.Game.ECS.Camp.Components.Commands.PlaceCamp(command.Position));

		_campPlaced = true;

		SetUIMode(_defaultUIMode);

		_viewModel.EnterPlaceCampModeCommand.IsVisible = false;
		_viewModel.EndTurnCommand.IsVisible = true;
	}


	private void OnHoveredTileChanged(HoveredTileChanged evt)
	{
		EcsService.SendEcsCommand(new HoveredTileChanged_Event(evt.Position));
	}

	#endregion


	private void SetUIMode(IUIMode mode)
	{
		if (_uiMode == mode)
			return;

		_uiMode.OnExit();
		_uiMode = mode;
		_uiMode.OnEnter();
	}
}



}
