using App.Game.Database;



namespace App.Application.Database.Presentation {



public interface IHumanTypePresentationRepository
{
	string GetName(HumanTypeId typeId);
}



}
