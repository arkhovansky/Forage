using Unity.Properties;

using Lib.UICore.Gui;

using App.Application.Flow.GameInstance.RunningGame.Models.UI;
using App.Game.Core.Query;
using App.Infrastructure.Common.Contracts.Database.Presentation;



namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels.Children {



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

	private readonly IRunningGame_UIModel_RO _uiModel;

	private readonly IResourceTypePresentationRepository _resourceTypePresentationRepository;



	public ResourceInfoVM(IMap map,
	                      IRunningGame_UIModel_RO uiModel,
	                      IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		_map = map;
		_uiModel = uiModel;
		_resourceTypePresentationRepository = resourceTypePresentationRepository;

		Name = string.Empty;
		RipenessPeriod = string.Empty;
	}


	public void Update()
	{
		if (!_uiModel.HighlightedTile.HasValue) {
			IsVisible = false;
			return;
		}

		var plantResource = _map.Get_PlantResource(_uiModel.HighlightedTile.Value);
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
