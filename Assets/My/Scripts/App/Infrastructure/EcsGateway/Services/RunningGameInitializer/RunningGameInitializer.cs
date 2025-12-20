using App.Application.Services;
using App.Game.Meta;
using App.Infrastructure.EcsGateway.Services.RunningGameInitializer.Features;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer {



public class RunningGameInitializer : IRunningGameInitializer
{
	private readonly IMapDataInitializer _mapDataInitializer;
	private readonly ITerrainInitializer _terrainInitializer;
	private readonly IResourcesInitializer _resourcesInitializer;
	private readonly IResourcePresentationInitializer _resourcePresentationInitializer;
	private readonly IGameTimeInitializer _gameTimeInitializer;
	private readonly IBandInitializer _bandInitializer;
	private readonly ISystemsInitializer _systemsInitializer;



	public RunningGameInitializer(
		IMapDataInitializer mapDataInitializer,
		ITerrainInitializer terrainInitializer,
		IResourcesInitializer resourcesInitializer,
		IResourcePresentationInitializer resourcePresentationInitializer,
		IGameTimeInitializer gameTimeInitializer,
		IBandInitializer bandInitializer,
		ISystemsInitializer systemsInitializer)
	{
		_mapDataInitializer = mapDataInitializer;
		_terrainInitializer = terrainInitializer;
		_resourcesInitializer = resourcesInitializer;
		_resourcePresentationInitializer = resourcePresentationInitializer;
		_gameTimeInitializer = gameTimeInitializer;
		_bandInitializer = bandInitializer;
		_systemsInitializer = systemsInitializer;
	}


	public void Initialize(ILocale locale)
	{
		_mapDataInitializer.Init(locale.Map);
		_terrainInitializer.Init(locale.TileTerrainTypes, locale.Map, locale.TilePhysicalInnerDiameter);
		_resourcesInitializer.Init(locale.ResourceAxialPositions, locale.ResourceTypes, locale.PotentialBiomass,
		                           locale.Map);
		_resourcePresentationInitializer.Init(locale.ResourceTypeIds);
		_gameTimeInitializer.Init(locale.StartYearPeriod);
		_bandInitializer.Init(locale.HumanTypeCounts);

		_systemsInitializer.Init();
	}
}



}
