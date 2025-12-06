using Lib.Grid;
using Lib.VisualGrid;

using App.Application.Services;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.Meta;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer {



public class RunningGameInitializer : IRunningGameInitializer
{
	private readonly ITerrainInitializer _terrainInitializer;
	private readonly IResourcesInitializer _resourcesInitializer;
	private readonly IResourcePresentationInitializer _resourcePresentationInitializer;
	private readonly IGameTimeInitializer _gameTimeInitializer;
	private readonly IBandInitializer _bandInitializer;
	private readonly ISystemsInitializer _systemsInitializer;
	private readonly HexLayout3D _grid;



	public RunningGameInitializer(
		ITerrainInitializer terrainInitializer,
		IResourcesInitializer resourcesInitializer,
		IResourcePresentationInitializer resourcePresentationInitializer,
		IGameTimeInitializer gameTimeInitializer,
		IBandInitializer bandInitializer,
		ISystemsInitializer systemsInitializer,
		HexLayout3D grid)
	{
		_terrainInitializer = terrainInitializer;
		_resourcesInitializer = resourcesInitializer;
		_resourcePresentationInitializer = resourcePresentationInitializer;
		_gameTimeInitializer = gameTimeInitializer;
		_bandInitializer = bandInitializer;
		_systemsInitializer = systemsInitializer;
		_grid = grid;
	}


	public void Initialize(ILocale locale)
	{
		InitMap(locale.Map);

		_terrainInitializer.Init(locale.TileTerrainTypes, locale.Map, locale.TilePhysicalInnerDiameter);
		_resourcesInitializer.Init(locale.ResourceAxialPositions, locale.ResourceTypes, locale.PotentialBiomass,
		                           locale.Map);
		_resourcePresentationInitializer.Init(locale.ResourceTypeIds);
		_gameTimeInitializer.Init(locale.StartYearPeriod);
		_bandInitializer.Init(locale.HumanTypeCounts);

		_systemsInitializer.Init();
	}


	private void InitMap(RectangularHexMap map)
	{
		EcsService.AddSingletonComponent(new Map(map));
		EcsService.AddSingletonComponent(new HexLayout3D_Component(_grid));
	}
}



}
