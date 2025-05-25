using Unity.Entities;



namespace App.Game.ECS.GameTime.Components {



public struct CurrentYearPeriod : IComponentData
{
	public App.Game.YearPeriod Value;


	public CurrentYearPeriod(YearPeriod value)
	{
		Value = value;
	}
}



}
