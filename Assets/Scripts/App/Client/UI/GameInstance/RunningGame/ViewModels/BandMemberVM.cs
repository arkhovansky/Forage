using Unity.Properties;



namespace App.Client.UI.GameInstance.RunningGame {



public class BandMemberVM
{
	[CreateProperty]
	public string Gender { get; set; } = string.Empty;

	[CreateProperty]
	public string Assignment { get; set; } = string.Empty;


	[DontCreateProperty]
	public int Id;
}



}
