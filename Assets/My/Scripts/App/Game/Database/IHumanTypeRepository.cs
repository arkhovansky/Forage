namespace App.Game.Database {



public enum Gender
{
	Male,
	Female
}


public class HumanType
{
	public Gender Gender;
}



public interface IHumanTypeRepository
{
	HumanType Get(HumanTypeId typeId);
}



}
