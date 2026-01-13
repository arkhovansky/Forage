using App.Game.Database;



namespace App.Infrastructure.Shared.Contracts.Database.Presentation {



public interface IResourceType_TextualPresentation_Repository
{
	string GetName(ResourceTypeId typeId);
}



}
