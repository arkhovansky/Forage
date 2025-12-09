using Unity.Properties;

using Lib.UICore.Mvvm;

using App.Application.Database.Presentation;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;



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

	private readonly IResourceTypePresentationRepository _resourceTypePresentationRepository;



	public ResourceInfoVM(IMap map,
	                      IScenePresentationModel_RO presentationModel,
	                      IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		_map = map;
		_presentationModel = presentationModel;
		_resourceTypePresentationRepository = resourceTypePresentationRepository;

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

		Name = _resourceTypePresentationRepository.GetName(resource.TypeId);
		PotentialBiomass = (uint) resource.PotentialBiomass;
		RipenessPeriod = resource.RipenessPeriod.Month.ToString();
		RipeBiomass = (uint) plantResource.Get_RipeBiomass();

		IsVisible = true;
	}
}



}
