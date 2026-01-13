using App.Game.Database;



namespace App.Infrastructure.Shared.Contracts.Database.Presentation {



public interface ITerrainType_TextualPresentation_Repository
{
	string GetName(TerrainTypeId typeId);
}



}
