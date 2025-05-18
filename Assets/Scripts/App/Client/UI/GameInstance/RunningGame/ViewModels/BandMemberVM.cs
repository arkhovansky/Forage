using Unity.Properties;



namespace App.Client.UI.GameInstance.RunningGame {



public class BandMemberVM
{
	[CreateProperty]
	public string Gender { get; set; }

	[CreateProperty]
	public string Assignment { get; set; }


	[DontCreateProperty]
	public int Id;
}



}
