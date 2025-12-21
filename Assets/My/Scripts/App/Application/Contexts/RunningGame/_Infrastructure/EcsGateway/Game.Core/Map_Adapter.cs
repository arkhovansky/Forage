using Unity.Entities;

using Lib.Grid;

using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Services;
using App.Game.Core.Query;
using App.Game.Database;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Terrain.Components;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Game.Core {



public class Map_Adapter : IMap
{
	private readonly IEcsHelper _ecsHelper;

	private EntityManager _entityManager
		= World.DefaultGameObjectInjectionWorld.EntityManager;

	//----------------------------------------------------------------------------------------------


	public Map_Adapter(IEcsHelper ecsHelper)
	{
		_ecsHelper = ecsHelper;
	}


	//----------------------------------------------------------------------------------------------
	// IMap implementation


	public TerrainTypeId Get_TerrainTypeId(AxialPosition tile)
	{
		var ecsMap = _ecsHelper.GetEcsMap();
		var tileEntity = ecsMap.GetTileEntity(tile);

		var terrainTileComponent = _entityManager.GetComponentData<TerrainTile>(tileEntity);
		return terrainTileComponent.TerrainType;
	}


	public IPlantResource? Get_PlantResource(AxialPosition tile)
	{
		var ecsMap = _ecsHelper.GetEcsMap();
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
