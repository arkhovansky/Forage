using Unity.Properties;

using Lib.UICore.Gui;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ViewModels.Children {



public class SelectCampLocationHintVM : IViewModel
{
	[CreateProperty]
	public bool IsVisible { get; set; }
}



}
