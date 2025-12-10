using Unity.Properties;

using Lib.UICore.Mvvm;



namespace App.Infrastructure.External.Presentation.GameInstance.RunningGame.ViewModels.Children {



public class SelectCampLocationHintVM : IViewModel
{
	[CreateProperty]
	public bool IsVisible { get; set; }
}



}
