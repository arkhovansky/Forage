using UnityEngine;

using App.Infrastructure.Common.Contracts.Database.Presentation;
using App.Infrastructure.External.Data.Database.Presentation.ScriptableObjects;



namespace App.Infrastructure.External.Data.Database.Presentation.Repositories {



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
