using Unity.Properties;

using Lib.UICore.Gui;



namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels.Children {



public class SelectCampLocationHintVM : IViewModel
{
	[CreateProperty]
	public bool IsVisible { get; set; }
}



}
