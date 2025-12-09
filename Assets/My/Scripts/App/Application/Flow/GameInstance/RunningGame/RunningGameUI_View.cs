using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Application.Flow.GameInstance.RunningGame.ViewModels;



namespace App.Application.Flow.GameInstance.RunningGame {



public class RunningGameUI_View : IView
{
	private readonly RunningGameUI_VM _viewModel;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;



	public RunningGameUI_View(RunningGameUI_VM viewModel,
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
