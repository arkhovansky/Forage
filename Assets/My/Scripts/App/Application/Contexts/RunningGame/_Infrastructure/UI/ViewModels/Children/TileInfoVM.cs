using Unity.Properties;

using Lib.UICore.Gui;

using App.Application.Contexts.RunningGame._Infrastructure.Shared.Contracts.Database.Presentation;
using App.Application.Contexts.RunningGame.Models.UI;
using App.Game.Core.Query;



namespace App.Application.Contexts.RunningGame._Infrastructure.UI.ViewModels.Children {



public class TileInfoVM : IViewModel
{
	[CreateProperty]
	public string TerrainType { get; private set; }


	public ResourceInfoVM ResourceInfoVM { get; }



	private readonly IMap _map;

	private readonly IRunningGame_UIModel_RO _uiModel;

	private readonly ITerrainTypePresentationRepository _terrainTypePresentationRepository;



	public TileInfoVM(IMap map,
	                  IRunningGame_UIModel_RO uiModel,
	                  ITerrainTypePresentationRepository terrainTypePresentationRepository,
	                  IResourceTypePresentationRepository resourceTypePresentationRepository)
	{
		_map = map;
		_uiModel = uiModel;
		_terrainTypePresentationRepository = terrainTypePresentationRepository;

		TerrainType = string.Empty;

		ResourceInfoVM = new ResourceInfoVM(map, uiModel, resourceTypePresentationRepository);
	}


	public void Update()
	{
		UpdateSelf();
		ResourceInfoVM.Update();
	}


	private void UpdateSelf()
	{
		if (!_uiModel.HighlightedTile.HasValue) {
			TerrainType = string.Empty;
			return;
		}

		var terrainTypeId = _map.Get_TerrainTypeId(_uiModel.HighlightedTile.Value);
		TerrainType = _terrainTypePresentationRepository.GetName(terrainTypeId);
	}
}



}
