using System;

using App.Game.Core;



namespace App.Application.Contexts.RunningGame_Gameplay.Models.UI.Impl {



public static class UIMode_Utils
{
	public static UIModeId CalculateUIMode(GamePhase gamePhase, bool is_CampPlacing_Mode)
	{
		return gamePhase switch {
			GamePhase.Arrival =>
				is_CampPlacing_Mode
					? UIModeId.CampPlacing
					: UIModeId.Arrival,
			GamePhase.InterPeriod => UIModeId.InterPeriod,
			GamePhase.PeriodRunning => UIModeId.PeriodRunning,

			_ => throw new ArgumentOutOfRangeException(nameof(gamePhase), gamePhase, null)
		};
	}
}



}
