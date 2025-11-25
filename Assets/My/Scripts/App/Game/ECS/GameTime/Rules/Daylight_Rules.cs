using System;

using Unity.Entities;
using UnityEngine;



namespace App.Game.ECS.GameTime.Rules {



[Serializable]
public struct Daylight_Rules : IComponentData
{
	[SerializeField] private int DaylightBeginHours;
	[SerializeField] private int DaylightEndHours;



	public bool IsDaylight(in Components.GameTime gameTime)
	{
		return gameTime.IntegerHours >= DaylightBeginHours &&
		       gameTime.IntegerHours < DaylightEndHours;
	}


	public bool GetDaylightEvent(in Components.GameTime gameTime, out bool isDaylight)
	{
		isDaylight = IsDaylight(gameTime);
		return gameTime.IntegerHoursChanged &&
		       (gameTime.IntegerHours == DaylightBeginHours || gameTime.IntegerHours == DaylightEndHours);
	}
}



}
