using Unity.Entities;
using Unity.Properties;

using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models;
using App.Game.Database;
using App.Game.ECS.Terrain.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class TileInfoVM : IViewModel
{
	[CreateProperty]
	public string TerrainType { get; private set; }


	public ResourceInfoVM ResourceInfoVM { get; }



	private readonly IScenePresentationModel _presentationModel;

	private readonly ITerrainTypeRepository _terrainTypeRepository;



	public TileInfoVM(IScenePresentationModel presentationModel,
	                  ITerrainTypeRepository terrainTypeRepository,
	                  IResourceTypeRepository resourceTypeRepository)
	{
		_presentationModel = presentationModel;
		_terrainTypeRepository = terrainTypeRepository;

		TerrainType = string.Empty;

		ResourceInfoVM = new ResourceInfoVM(presentationModel, resourceTypeRepository);
	}


	public void Update()
	{
		UpdateSelf();
		ResourceInfoVM.Update();
	}


	private void UpdateSelf()
	{
		if (!_presentationModel.HoveredTile.HasValue) {
			TerrainType = string.Empty;
			return;
		}

		var ecsMap = EcsService.GetEcsMap();
		var hoveredTileEntity = ecsMap.GetTileEntity(_presentationModel.HoveredTile.Value);

		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var terrainTileComponent = entityManager.GetComponentData<TerrainTile>(hoveredTileEntity);
		var terrainTypeId = terrainTileComponent.TerrainType;

		var terrainType = _terrainTypeRepository.Get(terrainTypeId);
		TerrainType = terrainType.Name;
	}
}



}
