using App.Application.PresentationDatabase;
using App.Game.Database;



namespace App.Infrastructure.External.Database.PresentationDatabase_Impl.Repositories {



public class HumanTypePresentationRepository : IHumanTypePresentationRepository
{
	public string GetName(HumanTypeId typeId)
	{
		return typeId.ToString();
	}
}



}
