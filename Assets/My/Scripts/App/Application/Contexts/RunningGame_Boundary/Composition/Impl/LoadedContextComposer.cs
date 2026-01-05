using UnityEngine;

using Lib.AppFlow;
using Lib.Grid;
using Lib.Grid.Spatial;
using Lib.Math;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.Repositories;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.DomainSettings.Repositories;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Locale;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Game.Core;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl;
using App.Application.Contexts.RunningGame_Boundary.Services;
using App.Game.Core;
using App.Infrastructure.EcsGateway.Contracts.Services;
using App.Infrastructure.EcsGateway.Services;
using App.Infrastructure.Shared.Contracts.Database.Presentation;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Application.Contexts.RunningGame_Boundary.Composition.Impl {



public class LoadedContextComposer : ILoadedContextComposer
{
	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;


	private readonly IContextData _contextData;

	//----------------------------------------------------------------------------------------------


	public LoadedContextComposer(IContextData contextData)
	{
		_contextData = contextData;
	}


	//----------------------------------------------------------------------------------------------
	// ILoadedContextComposer


	public void Compose(
		GameDatabase database,
		out ILocaleFactory localeFactory,
		out IRunningGameInitializer runningGameInitializer,
		out IRunningGameInstance runningGame)
	{
		var ecsHelper = new EcsHelper();

		var runningGameInstance = new RunningGameInstance(
			new World_Adapter(new Time_Adapter(ecsHelper), new Map_Adapter(ecsHelper), new Band_Adapter(ecsHelper)),
			_contextData.Get<IEcsSystems_Service>(), ecsHelper);
		runningGame = runningGameInstance;

		var localeRepository = new LocaleAssetRepository(database.Domain.Locales);
		localeFactory = new LocaleFactory(localeRepository);

		var gridLayout = new HexGridLayout_3D(
			new HexGridLayout(HexOrientation),
			new Matrix3x2(Vector3.right, Vector3.forward));

		var terrainTypePresentationRepository =
			new TerrainTypePresentationRepository(database.Presentation.TerrainTypes, gridLayout);
		var resourceTypePresentationRepository =
			new ResourceTypePresentationRepository(database.Presentation.ResourceTypes);
		var humanTypePresentationRepository = new HumanTypePresentationRepository();

		runningGameInitializer = Create_RunningGameInitializer(
			database, gridLayout, ecsHelper, terrainTypePresentationRepository, resourceTypePresentationRepository);


		_contextData.Add<IEcsHelper>(ecsHelper);
		_contextData.Add(gridLayout);
		_contextData.Add<ITerrainTypePresentationRepository>(terrainTypePresentationRepository);
		_contextData.Add<IResourceTypePresentationRepository>(resourceTypePresentationRepository);
		_contextData.Add<IHumanTypePresentationRepository>(humanTypePresentationRepository);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static IRunningGameInitializer Create_RunningGameInitializer(
		GameDatabase database,
		HexGridLayout_3D gridLayout,
		IEcsHelper ecsHelper,
		ITerrainTypePresentationRepository terrainTypePresentationRepository,
		IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		var mapDataInitializer = new MapDataInitializer(gridLayout, ecsHelper);

		var mapPresentationRepository = new MapPresentationRepository(database.Presentation);
		var terrainInitializer = new TerrainInitializer(
			gridLayout, terrainTypePresentationRepository, mapPresentationRepository, ecsHelper);

		var resourceTypeRepository = new ResourceTypeRepository(database.Domain.PlantResourceTypes);
		var resourcesInitializer = new ResourcesInitializer(
			resourceTypeRepository
#if !DOTS_DISABLE_DEBUG_NAMES
			, resourceTypePresentationRepository
#endif
		);

		var resourcePresentationInitializer =
			new ResourcePresentationInitializer(resourceTypePresentationRepository, ecsHelper);

		var gameTimeInitializer = new GameTimeInitializer(ecsHelper);

		var humanTypeRepository = new HumanTypeRepository(database.Domain.HumanTypes);
		var bandInitializer = new BandInitializer(humanTypeRepository);

		var systemParametersRepository = new SystemParametersRepository(database.Domain.SystemParameters);
		var domainSettingsRepository = new DomainSettingsRepository(database.DomainSettings);
		var systemParametersInitializer =
			new SystemsInitializer(systemParametersRepository, domainSettingsRepository);

		return new RunningGameInitializer(
			mapDataInitializer, terrainInitializer, resourcesInitializer, resourcePresentationInitializer,
			gameTimeInitializer, bandInitializer, systemParametersInitializer);
	}
}



}
