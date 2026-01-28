using Cysharp.Threading.Tasks;

using Lib.AppFlow;



namespace App.Application.Contexts.RunningGame_Gameplay {



/// <summary>
/// Handles gameplay using passed RunningGameInstance
/// </summary>
public class RunningGame_Gameplay_Context : Context
{
	private readonly ILoopComponent _runningGame;

	private readonly ILoopComponent _uiModel;

	private readonly ILoopComponent _sceneController;

	private readonly ILoopComponent _resourceMarkers_PresentationLayer;

	private readonly ILoopComponent _screenUI_VM;
	private readonly Lib.UICore.Gui.IView _screenUI_View;

	private readonly UniMob.LifetimeController _atomLifetimeController;

	//----------------------------------------------------------------------------------------------


	public RunningGame_Gameplay_Context(
		ILoopComponent runningGame,
		ILoopComponent uiModel,
		IController controller,
		ILoopComponent sceneController,
		IView camera_View,
		IView worldUI_View,
		ILoopComponent resourceMarkers_PresentationLayer,
		ILoopComponent screenUI_VM,
		Lib.UICore.Gui.IView screenUI_View,
		UniMob.LifetimeController atomLifetimeController)
	{
		_runningGame = runningGame;
		_uiModel = uiModel;
		_sceneController = sceneController;
		_resourceMarkers_PresentationLayer = resourceMarkers_PresentationLayer;
		_screenUI_VM = screenUI_VM;
		_screenUI_View = screenUI_View;
		_atomLifetimeController = atomLifetimeController;

		Controller = controller;

		base.AddView(camera_View);
		base.AddView(worldUI_View);
	}


	public override UniTask Start()
	{
		_runningGame.Start();
		Controller!.Start();
		_uiModel.Start();

		return UniTask.CompletedTask;
	}


	protected override void DoUpdate()
	{
		// Controller should be updated first to handle Model-originating state changes in previous frame's LateUpdate()
		UpdateController();
		_sceneController.Update();
	}


	protected override void DoLateUpdate()
	{
		// Update model first because it can change UI mode
		_runningGame.LateUpdate();

		_resourceMarkers_PresentationLayer.LateUpdate();
		_screenUI_VM.LateUpdate();
	}


	public override void Destroy()
	{
		_atomLifetimeController.Dispose();
	}
}



}
