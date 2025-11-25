using System;

using Unity.Entities;



namespace App.Game.ECS.GameTime.Rules {



[Serializable]
public struct GameTime_Rules : IComponentData
{
	public int DaysInYearPeriod;
}



}
