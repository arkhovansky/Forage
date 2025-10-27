using Unity.Properties;



namespace App.Client.UI.GameInstance.RunningGame {



public class BandMemberVM
{
	[CreateProperty]
	public string Gender { get; set; } = string.Empty;

	[CreateProperty]
	public string Goal { get; set; } = string.Empty;

	[CreateProperty]
	public string Activity { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageForagingHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageGatheringHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageMovingHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageLeisureHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageSleepingHours { get; set; } = string.Empty;


	[DontCreateProperty]
	public int Id;
}



}
