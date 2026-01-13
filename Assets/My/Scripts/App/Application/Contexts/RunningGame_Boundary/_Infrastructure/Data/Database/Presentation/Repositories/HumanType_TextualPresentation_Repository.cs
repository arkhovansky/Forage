using App.Game.Database;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class HumanType_TextualPresentation_Repository : IHumanType_TextualPresentation_Repository
{
	public string GetName(HumanTypeId typeId)
	{
		return typeId.ToString();
	}
}



}
