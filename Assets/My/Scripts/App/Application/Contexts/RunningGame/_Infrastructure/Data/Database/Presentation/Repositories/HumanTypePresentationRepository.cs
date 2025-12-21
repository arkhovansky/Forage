using App.Application.Contexts.RunningGame._Infrastructure.Shared.Contracts.Database.Presentation;
using App.Game.Database;



namespace App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Presentation.Repositories {



public class HumanTypePresentationRepository : IHumanTypePresentationRepository
{
	public string GetName(HumanTypeId typeId)
	{
		return typeId.ToString();
	}
}



}
