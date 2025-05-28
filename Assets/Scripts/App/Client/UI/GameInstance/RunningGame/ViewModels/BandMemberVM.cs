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


	[DontCreateProperty]
	public int Id;
}



}
