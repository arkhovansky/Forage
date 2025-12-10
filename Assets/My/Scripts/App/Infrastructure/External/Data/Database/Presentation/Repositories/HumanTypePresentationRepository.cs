using App.Game.Database;
using App.Infrastructure.Common.Contracts.Database.Presentation;



namespace App.Infrastructure.External.Data.Database.Presentation.Repositories {



public class HumanTypePresentationRepository : IHumanTypePresentationRepository
{
	public string GetName(HumanTypeId typeId)
	{
		return typeId.ToString();
	}
}



}
