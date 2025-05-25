using Unity.Entities;



namespace App.Game.ECS.GameTime.Components.Commands {



public struct InitYearPeriod : IComponentData
{
	public YearPeriod YearPeriod;


	public InitYearPeriod(YearPeriod yearPeriod)
	{
		YearPeriod = yearPeriod;
	}
}



}
