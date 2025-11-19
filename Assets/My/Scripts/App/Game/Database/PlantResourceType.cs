using System;

using App.Game.ECS.GameTime.Components;



namespace App.Game.Database {



[Serializable]
public class PlantResourceType
{
	public ResourceTypeId Id;

	public string Name = null!;

	public YearPeriod RipenessPeriod;
}



}
