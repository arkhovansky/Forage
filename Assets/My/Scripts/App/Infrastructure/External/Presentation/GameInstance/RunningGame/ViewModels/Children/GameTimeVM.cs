using Unity.Properties;

using JetBrains.Annotations;

using Lib.UICore.Mvvm;

using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;



namespace App.Infrastructure.External.Presentation.GameInstance.RunningGame.ViewModels.Children {



public class GameTimeVM : IViewModel
{
	[CreateProperty]
	public string GameTime { [UsedImplicitly] get; private set; } = string.Empty;


	private readonly ITime _time;

	//----------------------------------------------------------------------------------------------


	public GameTimeVM(ITime time)
	{
		_time = time;
	}


	public void Update()
	{
		var gameTime = _time.Get_Time();
		bool daylight = _time.Get_IsDaylight();

		var partOfDay = daylight ? "Day" : "Night";
		GameTime = $"{gameTime.YearPeriod.Month.ToString()}   Day: {gameTime.Day}   Hour: {(uint)gameTime.Hours}   " +
		           $"({partOfDay})";
	}
}



}
