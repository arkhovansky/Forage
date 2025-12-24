using UnityEngine;

using Lib.AppFlow;
using Lib.AppFlow.Resolution;
using Lib.Grid.Spatial;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.EcsGateway.Views;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ViewModels;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.Views;
using App.Application.Contexts.RunningGame_Gameplay.Controller;
using App.Application.Contexts.RunningGame_Gameplay.Models.UI.Impl;
using App.Game.Core;
using App.Infrastructure.EcsGateway.Contracts.Services;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Gameplay {



public class EntryPoint : IContextEntryPoint
{
	public static readonly EntryPoint Instance;

	static EntryPoint()
	{
		Instance = new EntryPoint();
	}


	//----------------------------------------------------------------------------------------------
	// IContextEntryPoint


	public IContextCapability Get_CapabilityDescriptor(IContextCapability_Builder builder)
	{
		return builder
			.Subject<IRunningGameInstance>()
			.Build();
	}


	public IContext Create(IContextRequest request, IContextData contextData)
	{
		var runningGameInstance = request.GetSubject<IRunningGameInstance>();

		return Create(runningGameInstance, contextData);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private IContext Create(IRunningGameInstance runningGameInstance, IContextData contextData)
	{
		var uiModel = new RunningGame_UIModel();

		var controller = new RunningGameController(runningGameInstance, uiModel);

		var map = runningGameInstance.World.Map.Get_GridMap();
		var gridLayout = contextData.Get<HexGridLayout_3D>();
		var spatialMap = new Spatial_RectangularHexMap_3D(map, gridLayout);
		var sceneViewController = new SceneViewController(Camera.main!, spatialMap);

		var worldUI_View = new WorldUI_View(contextData.Get<IEcsHelper>());

		var gui = contextData.Get<IGui>();

		var screenUI_VM = new RunningGame_ScreenUI_VM(
			runningGameInstance,
			uiModel,
			contextData.Get<ITerrainTypePresentationRepository>(),
			contextData.Get<IResourceTypePresentationRepository>(),
			contextData.Get<IHumanTypePresentationRepository>());
		var screenUI_View = new RunningGame_ScreenUI_View(
			screenUI_VM,
			gui, contextData.Get<IVvmBinder>());

		var context = new RunningGame_Gameplay_Context(
			(ILoopComponent) runningGameInstance,
			uiModel,
			controller,
			sceneViewController,
			sceneViewController,
			worldUI_View,
			screenUI_VM,
			screenUI_View);

		uiModel.Init_PresentationEvent_Emitter(context);
		controller.Init_Command_Emitter(context);
		sceneViewController.Init_InputEvent_Emitter(context);
		screenUI_VM.Init_Command_Emitter(context);

		gui.AddView(screenUI_View);

		return context;
	}
}



}
