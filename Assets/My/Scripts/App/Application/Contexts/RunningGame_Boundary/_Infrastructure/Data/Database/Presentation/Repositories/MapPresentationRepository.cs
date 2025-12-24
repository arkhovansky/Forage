using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class MapPresentationRepository : IMapPresentationRepository
{
	private readonly PresentationDatabase _database;



	public MapPresentationRepository(PresentationDatabase database)
	{
		_database = database;
	}


	public Material Get_GridLinesMaterial()
		=> _database.TerrainGridMaterial;
}



}
