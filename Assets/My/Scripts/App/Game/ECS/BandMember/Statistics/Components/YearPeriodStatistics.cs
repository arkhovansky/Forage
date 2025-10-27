using Unity.Entities;



namespace App.Game.ECS.BandMember.Statistics.Components {



public struct YearPeriodStatistics : IComponentData
{
	public float TotalForagingHours;
	public float TotalGatheringHours;
	public float TotalMovingHours;
	public float TotalLeisureHours;
	public float TotalSleepingHours;

	public uint DayCount;

	public float AverageForagingHours;
	public float AverageGatheringHours;
	public float AverageMovingHours;
	public float AverageLeisureHours;
	public float AverageSleepingHours;



	public void AddDayStatistics(in DayStatistics dayStatistics)
	{
		TotalForagingHours += dayStatistics.ForagingHours;
		TotalGatheringHours += dayStatistics.GatheringHours;
		TotalMovingHours += dayStatistics.MovingHours;
		TotalLeisureHours += dayStatistics.LeisureHours;
		TotalSleepingHours += dayStatistics.SleepingHours;

		DayCount++;

		CalculateAverages();
	}


	private void CalculateAverages()
	{
		AverageForagingHours = TotalForagingHours / DayCount;
		AverageGatheringHours = TotalGatheringHours / DayCount;
		AverageMovingHours = TotalMovingHours / DayCount;
		AverageLeisureHours = TotalLeisureHours / DayCount;
		AverageSleepingHours = TotalSleepingHours / DayCount;
	}
}



}
