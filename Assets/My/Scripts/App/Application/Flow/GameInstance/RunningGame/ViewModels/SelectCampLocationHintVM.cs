using Unity.Properties;

using App.Application.Framework.UICore.Mvvm;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class SelectCampLocationHintVM : IViewModel
{
	[CreateProperty]
	public bool IsVisible { get; set; }
}



}
