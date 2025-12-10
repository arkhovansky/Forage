using App.Game.Database;



namespace App.Infrastructure.Common.Contracts.Database.Presentation {



public interface IHumanTypePresentationRepository
{
	string GetName(HumanTypeId typeId);
}



}
