using Unity.Properties;

using Lib.UICore.Mvvm;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class SelectCampLocationHintVM : IViewModel
{
	[CreateProperty]
	public bool IsVisible { get; set; }
}



}
