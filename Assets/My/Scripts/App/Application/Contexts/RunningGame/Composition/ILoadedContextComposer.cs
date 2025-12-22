using Lib.AppFlow;
using Lib.Grid;

using App.Application.Contexts.RunningGame.Services;



namespace App.Application.Contexts.RunningGame.Composition {



public interface ILoadedContextComposer
{
	void Compose(RunningGameContext context,
	             out ILocaleFactory localeFactory,
	             out IRunningGameInitializer runningGameInitializer,
	             out ILoopComponent runningGame,
	             out ILoopComponent uiModel,
	             out IController controller,
	             out IView worldUI_View,
	             out ILoopComponent screenUI_VM,
	             out Lib.UICore.Gui.IView screenUI_View);

	IView Create_SceneViewController(RectangularHexMap map,
	                                 RunningGameContext context);
}



}
