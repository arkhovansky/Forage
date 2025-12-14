using Unity.Properties;

using Lib.UICore.Mvvm;

using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Game.Core.Query;
using App.Infrastructure.Common.Contracts.Database.Presentation;



namespace App.Infrastructure.External.UI.GameInstance.RunningGame.ViewModels.Children {



public class TileInfoVM : IViewModel
{
	[CreateProperty]
	public string TerrainType { get; private set; }


	public ResourceInfoVM ResourceInfoVM { get; }



	private readonly IMap _map;

	private readonly IRunningGame_PresentationModel_RO _presentationModel;

	private readonly ITerrainTypePresentationRepository _terrainTypePresentationRepository;



	public TileInfoVM(IMap map,
	                  IRunningGame_PresentationModel_RO presentationModel,
	                  ITerrainTypePresentationRepository terrainTypePresentationRepository,
	                  IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		_map = map;
		_presentationModel = presentationModel;
		_terrainTypePresentationRepository = terrainTypePresentationRepository;

		TerrainType = string.Empty;

		ResourceInfoVM = new ResourceInfoVM(map, presentationModel, resourceTypePresentationRepository);
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

		var terrainTypeId = _map.Get_TerrainTypeId(_presentationModel.HoveredTile.Value);
		TerrainType = _terrainTypePresentationRepository.GetName(terrainTypeId);
	}
}



}
