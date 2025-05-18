namespace App.Services.BandMembers {



public enum Gender
{
	Male,
	Female
}


public class BandMemberType
{
	public Gender Gender;

	public float GatheringSpeed;
}



public interface IBandMemberTypeRepository
{
	BandMemberType Get(uint typeId);
}



}
