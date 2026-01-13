using App.Game.Database;



namespace App.Infrastructure.Shared.Contracts.Database.Presentation {



public interface IHumanType_TextualPresentation_Repository
{
	string GetName(HumanTypeId typeId);
}



}
