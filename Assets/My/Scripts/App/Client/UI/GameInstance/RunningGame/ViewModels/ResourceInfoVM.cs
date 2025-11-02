using Unity.Entities;
using Unity.Properties;

using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.Resource.Plant.Components;
using App.Services.Resources;



namespace App.Client.UI.GameInstance.RunningGame.ViewModels {



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

		RipeBiomass = (uint) entityManager.GetComponentData<RipeBiomass>(resourceEntity).Value;

		IsVisible = true;
	}


	public void Hide()
	{
		IsVisible = false;
	}
}



}
