using App.Game.Ecs.Systems;

using Unity.Entities;

using Lib.VisualGrid;

using App.Game.Meta;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Services {



public class GameService : IGameService
{
	private readonly ITerrainInitializer _terrainInitializer;
	private readonly IResourcesInitializer _resourcesInitializer;
	private readonly IGameTimeInitializer _gameTimeInitializer;
	private readonly IBandInitializer _bandInitializer;
	private readonly HexLayout _grid;
	private readonly ResourceTypePresentationRepository _resourceTypePresentationRepository;



	public GameService(
		ITerrainInitializer terrainInitializer,
		IResourcesInitializer resourcesInitializer,
		IGameTimeInitializer gameTimeInitializer,
		IBandInitializer bandInitializer,
		HexLayout grid,
		ResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		_terrainInitializer = terrainInitializer;
		_resourcesInitializer = resourcesInitializer;
		_gameTimeInitializer = gameTimeInitializer;
		_bandInitializer = bandInitializer;
		_grid = grid;
		_resourceTypePresentationRepository = resourceTypePresentationRepository;
	}


	public void PopulateWorld(IScene scene)
	{
		_terrainInitializer.Create(scene.TileTerrainTypes, scene.TileAxialPositions);
		_resourcesInitializer.Init(scene.ResourceAxialPositions, scene.ResourceTypes, scene.PotentialBiomass);
		_gameTimeInitializer.Init(scene.StartYearPeriod);
		_bandInitializer.Init(scene.BandMemberTypeCounts);

		var plantResourcePresentation =
			World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlantResourcePresentation>();
		plantResourcePresentation.InitForScene(_grid, _resourceTypePresentationRepository, scene.ResourceTypes);
	}
}



}
