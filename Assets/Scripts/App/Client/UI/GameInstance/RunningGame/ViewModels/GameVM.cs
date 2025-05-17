using Unity.Collections;
using Unity.Entities;
using Unity.Properties;

using Lib.Grid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Game.Ecs.Components;
using App.Game.Ecs.Components.Singletons.HoveredTile;
using App.Game.Ecs.Components.Singletons.YearPeriod;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public class GameVM : IViewModel
{
	[CreateProperty]
	public string Month { get; private set; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public EndTurn_CommandVM EndTurnCommand { get; }



	public GameVM(IController controller,
	              ICommandRouter commandRouter,
	              ITerrainTypeRepository terrainTypeRepository, IResourceTypeRepository resourceTypeRepository)
	{
		Month = string.Empty;

		TileInfoVM = new TileInfoVM(terrainTypeRepository, resourceTypeRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		EndTurnCommand = new EndTurn_CommandVM(
			() => commandRouter.EmitCommand(new EndTurnCommand(), controller));
	}


	public void Update()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<CurrentYearPeriod>());
		var yearPeriod = query.GetSingleton<CurrentYearPeriod>();

		Month = yearPeriod.Value.Month.ToString();


		TileInfoVM.Update();
	}
}



public class TileInfoVM : IViewModel
{
	[CreateProperty]
	public string TerrainType { get; private set; }


	public ResourceInfoVM ResourceInfoVM { get; }



	private readonly ITerrainTypeRepository _terrainTypeRepository;



	public TileInfoVM(ITerrainTypeRepository terrainTypeRepository,
	                  IResourceTypeRepository resourceTypeRepository)
	{
		_terrainTypeRepository = terrainTypeRepository;

		TerrainType = string.Empty;

		ResourceInfoVM = new ResourceInfoVM(resourceTypeRepository);
	}


	public void Update()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<HoveredTileEntity>());

		if (query.TryGetSingleton<HoveredTileEntity>(out var hoveredTileEntityComponent)) {
			var hoveredTileEntity = hoveredTileEntityComponent.Entity;

			var terrainTileComponent = entityManager.GetComponentData<TerrainTile>(hoveredTileEntity);
			var terrainTypeId = terrainTileComponent.TerrainType;

			var terrainType = _terrainTypeRepository.Get(terrainTypeId);
			TerrainType = terrainType.Name;


			var position = entityManager.GetComponentData<TilePosition>(hoveredTileEntity).Position;

			if (TryGetResource(position, out Entity resourceEntity))
				ResourceInfoVM.Show(resourceEntity);
			else
				ResourceInfoVM.Hide();
		}
		else {
			TerrainType = string.Empty;
			ResourceInfoVM.Hide();
		}
	}


	private bool TryGetResource(AxialPosition position, out Entity resourceEntity)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(
			ComponentType.ReadOnly<TilePosition>(),
			ComponentType.ReadOnly<PlantResource>());

		var positions = query.ToComponentDataArray<TilePosition>(Allocator.Temp);
		var entities = query.ToEntityArray(Allocator.Temp);

		for (var i = 0; i < positions.Length; i++) {
			if (positions[i].Position == position) {
				resourceEntity = entities[i];
				return true;
			}
		}

		resourceEntity = Entity.Null;
		return false;
	}
}



public class ResourceInfoVM : IViewModel
{
	[CreateProperty]
	public bool IsVisible { get; private set; }

	[CreateProperty]
	public string Name { get; private set; }

	[CreateProperty]
	public uint PotentialBiomass { get; private set; }

	[CreateProperty]
	public string RipenessPeriod { get; private set; }

	[CreateProperty]
	public uint RipeBiomass { get; private set; }



	private readonly IResourceTypeRepository _resourceTypeRepository;



	public ResourceInfoVM(IResourceTypeRepository resourceTypeRepository)
	{
		_resourceTypeRepository = resourceTypeRepository;

		Name = string.Empty;
		RipenessPeriod = string.Empty;
	}


	public void Show(Entity resourceEntity)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var resource = entityManager.GetComponentData<PlantResource>(resourceEntity);

		var resourceType = _resourceTypeRepository.Get(resource.TypeId);

		Name = resourceType.Name;
		PotentialBiomass = (uint) resource.PotentialBiomass;
		RipenessPeriod = resource.RipenessPeriod.Month.ToString();

		RipeBiomass = (uint) entityManager.GetComponentData<RemainingRipeBiomass>(resourceEntity).Value;

		IsVisible = true;
	}


	public void Hide()
	{
		IsVisible = false;
	}
}




}
