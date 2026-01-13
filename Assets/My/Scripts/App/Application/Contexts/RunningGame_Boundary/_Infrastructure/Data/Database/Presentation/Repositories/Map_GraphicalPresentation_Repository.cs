using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class Map_GraphicalPresentation_Repository : IMap_GraphicalPresentation_Repository
{
	private readonly PresentationDatabase _database;



	public Map_GraphicalPresentation_Repository(PresentationDatabase database)
	{
		_database = database;
	}


	public Material Get_GridLinesMaterial()
		=> _database.TerrainGridMaterial;
}



}
