using Unity.Entities;
using Unity.Properties;

using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models;
using App.Game.Database;
using App.Game.ECS.Resource.Plant.Components;
using App.Infrastructure.EcsGateway.Services;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



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



	private readonly IScenePresentationModel _presentationModel;

	private readonly IResourceTypeRepository _resourceTypeRepository;



	public ResourceInfoVM(IScenePresentationModel presentationModel,
	                      IResourceTypeRepository resourceTypeRepository)
	{
		_presentationModel = presentationModel;
		_resourceTypeRepository = resourceTypeRepository;

		Name = string.Empty;
		RipenessPeriod = string.Empty;
	}


	public void Update()
	{
		if (!_presentationModel.HoveredTile.HasValue) {
			IsVisible = false;
			return;
		}

		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var ecsMap = EcsService.GetEcsMap();
		var hoveredTileEntity = ecsMap.GetTileEntity(_presentationModel.HoveredTile.Value);

		if (!entityManager.HasComponent<TilePlantResource>(hoveredTileEntity)) {
			IsVisible = false;
			return;
		}

		var resourceEntity = entityManager.GetComponentData<TilePlantResource>(hoveredTileEntity).ResourceEntity;
		if (resourceEntity == Entity.Null) {
			IsVisible = false;
			return;
		}

		var resource = entityManager.GetComponentData<PlantResource>(resourceEntity);
		var resourceType = _resourceTypeRepository.Get(resource.TypeId);

		Name = resourceType.Name;
		PotentialBiomass = (uint) resource.PotentialBiomass;
		RipenessPeriod = resource.RipenessPeriod.Month.ToString();
		RipeBiomass = (uint) entityManager.GetComponentData<RipeBiomass>(resourceEntity).Value;

		IsVisible = true;
	}
}



}
