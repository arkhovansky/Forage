using Lib.UICore.Gui;
using Lib.UICore.Mvvm;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.Screen.Views {



public class RunningGame_ScreenUI_View : IView
{
	private readonly IViewModel _viewModel;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;



	public RunningGame_ScreenUI_View(IViewModel viewModel,
	                                 IGui gui, IVvmBinder vvmBinder)
	{
		_viewModel = viewModel;

		_gui = gui;
		_vvmBinder = vvmBinder;
	}


	public void Build()
	{
		var visualNode = _gui.RootVisualNode;

		_gui.SetVisualResource(visualNode, "Game/Game");

		_vvmBinder.Bind(visualNode, _viewModel);
	}
}



}
