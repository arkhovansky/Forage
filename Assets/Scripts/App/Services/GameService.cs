using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.Map.Components.Singletons;
using App.Game.Meta;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Services {



public class GameService : IGameService
{
	private readonly ITerrainInitializer _terrainInitializer;
	private readonly IResourcesInitializer _resourcesInitializer;
	private readonly IResourcePresentationInitializer _resourcePresentationInitializer;
	private readonly IGameTimeInitializer _gameTimeInitializer;
	private readonly IBandInitializer _bandInitializer;
	private readonly HexLayout3D _grid;



	public GameService(
		ITerrainInitializer terrainInitializer,
		IResourcesInitializer resourcesInitializer,
		IResourcePresentationInitializer resourcePresentationInitializer,
		IGameTimeInitializer gameTimeInitializer,
		IBandInitializer bandInitializer,
		HexLayout3D grid)
	{
		_terrainInitializer = terrainInitializer;
		_resourcesInitializer = resourcesInitializer;
		_resourcePresentationInitializer = resourcePresentationInitializer;
		_gameTimeInitializer = gameTimeInitializer;
		_bandInitializer = bandInitializer;
		_grid = grid;
	}


	public void PopulateWorld(IScene scene)
	{
		InitMap(scene.Map);

		_terrainInitializer.Init(scene.TileTerrainTypes, scene.Map, scene.TilePhysicalInnerDiameter);
		_resourcesInitializer.Init(scene.ResourceAxialPositions, scene.ResourceTypes, scene.PotentialBiomass,
		                           scene.Map);
		_resourcePresentationInitializer.Init(scene.ResourceTypeIds);
		_gameTimeInitializer.Init(scene.StartYearPeriod);
		_bandInitializer.Init(scene.BandMemberTypeCounts);
	}


	private void InitMap(RectangularHexMap map)
	{
		EcsService.AddSingletonComponent(new Map(map));
		EcsService.AddSingletonComponent(new HexLayout3D_Component(_grid));
	}
}



}
