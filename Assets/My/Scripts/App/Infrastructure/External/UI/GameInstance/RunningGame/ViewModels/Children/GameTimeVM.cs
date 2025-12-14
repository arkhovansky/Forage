using Unity.Properties;

using JetBrains.Annotations;

using Lib.UICore.Gui;

using App.Game.Core.Query;



namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels.Children {



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
