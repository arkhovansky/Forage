using Unity.Entities;

using Lib.Grid;

using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Game.Database;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Terrain.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Infrastructure.EcsGateway.Models.Domain {



public class Map_Adapter : IMap
{
	private EntityManager _entityManager
		= World.DefaultGameObjectInjectionWorld.EntityManager;



	public TerrainTypeId Get_TerrainTypeId(AxialPosition tile)
	{
		var ecsMap = EcsService.GetEcsMap();
		var tileEntity = ecsMap.GetTileEntity(tile);

		var terrainTileComponent = _entityManager.GetComponentData<TerrainTile>(tileEntity);
		return terrainTileComponent.TerrainType;
	}


	public IPlantResource? Get_PlantResource(AxialPosition tile)
	{
		var ecsMap = EcsService.GetEcsMap();
		var tileEntity = ecsMap.GetTileEntity(tile);

		if (!_entityManager.HasComponent<TilePlantResource>(tileEntity))
			return null;

		var resourceEntity = _entityManager.GetComponentData<TilePlantResource>(tileEntity).ResourceEntity;
		if (resourceEntity == Entity.Null)
			return null;

		return new PlantResource_Adapter(resourceEntity);
	}
}



}
