using Unity.Entities;



namespace App.Game.ECS.BandMember.Statistics.Components {



public struct DayStatistics : IComponentData
{
	public float ForagingHours;

	public float GatheringHours;

	public float MovingHours;

	public float LeisureHours;

	public float SleepingHours;



	public void Reset()
	{
		ForagingHours = 0;
		GatheringHours = 0;
		MovingHours = 0;
		LeisureHours = 0;
		SleepingHours = 0;
	}
}



}
