using Unity.Properties;

using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Game.Database;



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



	private readonly IMap _map;

	private readonly IScenePresentationModel_RO _presentationModel;

	private readonly IResourceTypeRepository _resourceTypeRepository;



	public ResourceInfoVM(IMap map,
	                      IScenePresentationModel_RO presentationModel,
	                      IResourceTypeRepository resourceTypeRepository)
	{
		_map = map;
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

		var plantResource = _map.Get_PlantResource(_presentationModel.HoveredTile.Value);
		if (plantResource == null) {
			IsVisible = false;
			return;
		}

		var resource = plantResource.Get_StaticData();
		var resourceType = _resourceTypeRepository.Get(resource.TypeId);

		Name = resourceType.Name;
		PotentialBiomass = (uint) resource.PotentialBiomass;
		RipenessPeriod = resource.RipenessPeriod.Month.ToString();
		RipeBiomass = (uint) plantResource.Get_RipeBiomass();

		IsVisible = true;
	}
}



}
