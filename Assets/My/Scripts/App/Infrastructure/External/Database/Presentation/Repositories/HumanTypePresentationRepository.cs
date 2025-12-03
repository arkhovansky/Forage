using App.Application.Database.Presentation;
using App.Game.Database;



namespace App.Infrastructure.External.Database.Presentation.Repositories {



public class HumanTypePresentationRepository : IHumanTypePresentationRepository
{
	public string GetName(HumanTypeId typeId)
	{
		return typeId.ToString();
	}
}



}
