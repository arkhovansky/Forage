namespace App.Game.Database {



public class ResourceType
{
	public string Name;

	public YearPeriod RipenessPeriod;


	public ResourceType(string name, YearPeriod ripenessPeriod)
	{
		Name = name;
		RipenessPeriod = ripenessPeriod;
	}
}



public interface IResourceTypeRepository
{
	ResourceType Get(uint resourceTypeId);
}



}
