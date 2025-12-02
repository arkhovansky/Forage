using App.Game.Database;



namespace App.Application.PresentationDatabase {



public interface IHumanTypePresentationRepository
{
	string GetName(HumanTypeId typeId);
}



}
