using App.Game.Database;



namespace App.Infrastructure.Shared.Contracts.Database.Presentation {



public interface IHumanTypePresentationRepository
{
	string GetName(HumanTypeId typeId);
}



}
