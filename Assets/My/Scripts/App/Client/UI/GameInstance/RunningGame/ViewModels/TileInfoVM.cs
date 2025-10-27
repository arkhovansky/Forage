using Unity.Collections;
using Unity.Entities;
using Unity.Properties;

using Lib.Grid;

using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Terrain.Components;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



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


			var position = entityManager.GetComponentData<MapPosition>(hoveredTileEntity).Value;

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
			ComponentType.ReadOnly<MapPosition>(),
			ComponentType.ReadOnly<PlantResource>());

		var positions = query.ToComponentDataArray<MapPosition>(Allocator.Temp);
		var entities = query.ToEntityArray(Allocator.Temp);

		for (var i = 0; i < positions.Length; i++) {
			if (positions[i] == position) {
				resourceEntity = entities[i];
				return true;
			}
		}

		resourceEntity = Entity.Null;
		return false;
	}
}



}
