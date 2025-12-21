using App.Game.Database;



namespace App.Application.Contexts.RunningGame._Infrastructure.Shared.Contracts.Database.Presentation {



public interface IHumanTypePresentationRepository
{
	string GetName(HumanTypeId typeId);
}



}
