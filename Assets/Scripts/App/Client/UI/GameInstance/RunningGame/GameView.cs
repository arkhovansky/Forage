using App.Client.Framework.UICore.LowLevel;
using App.Client.Framework.UICore.Mvvm;



namespace App.Client.UI.GameInstance.RunningGame {



public class GameView : IView
{
	private readonly GameVM _viewModel;

	private readonly IGui _gui;
	private readonly IVvmBinder _vvmBinder;



	public GameView(GameVM viewModel,
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
