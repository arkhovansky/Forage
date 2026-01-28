using UnityEngine;

using Lib.AppFlow;
using Lib.AppFlow.Resolution;
using Lib.Grid.Spatial;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.EcsGateway.Views;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.Screen.ViewModels;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.Screen.Views;
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

		var gridLayout = contextData.Get<HexGridLayout_3D>();
		var camera = Camera.main!;

		var atomLifetimeController = new UniMob.LifetimeController();

		var map = runningGameInstance.World.Map.Get_GridMap();
		var spatialMap = new Spatial_RectangularHexMap_3D(map, gridLayout);
		var sceneViewController = new SceneViewController(camera, spatialMap, atomLifetimeController.Lifetime);

		var worldUI_View = new WorldUI_View(contextData.Get<IEcsHelper>());

		var resourceMarkers_PresentationLayer =
			ResourceMarkers_PresentationLayer_Composer.Compose(
				runningGameInstance.World.PlantResources, runningGameInstance.World.Time,
				in gridLayout,
				contextData.Get<IResourceMarker_Config_Repository>(),
				contextData.Get<IResourceType_Icon_Repository>(),
				camera,
				sceneViewController.CameraTransform_Atom,
				atomLifetimeController.Lifetime);

		var gui = contextData.Get<IGui>();

		var screenUI_VM = new RunningGame_ScreenUI_VM(
			runningGameInstance,
			uiModel,
			contextData.Get<ITerrainType_TextualPresentation_Repository>(),
			contextData.Get<IResourceType_TextualPresentation_Repository>(),
			contextData.Get<IHumanType_TextualPresentation_Repository>());
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
			resourceMarkers_PresentationLayer,
			screenUI_VM,
			screenUI_View,
			atomLifetimeController);

		uiModel.Init_PresentationEvent_Emitter(context);
		controller.Init_Command_Emitter(context);
		sceneViewController.Init_InputEvent_Emitter(context);
		screenUI_VM.Init_Command_Emitter(context);

		gui.AddView(screenUI_View);

		return context;
	}
}



}
