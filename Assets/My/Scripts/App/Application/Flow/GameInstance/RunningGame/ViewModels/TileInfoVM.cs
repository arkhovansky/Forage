using Unity.Properties;

using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Application.Flow.GameInstance.RunningGame.Models.Presentation;
using App.Game.Database;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class TileInfoVM : IViewModel
{
	[CreateProperty]
	public string TerrainType { get; private set; }


	public ResourceInfoVM ResourceInfoVM { get; }



	private readonly IMap _map;

	private readonly IScenePresentationModel_RO _presentationModel;

	private readonly ITerrainTypeRepository _terrainTypeRepository;



	public TileInfoVM(IMap map,
	                  IScenePresentationModel_RO presentationModel,
	                  ITerrainTypeRepository terrainTypeRepository,
	                  IResourceTypeRepository resourceTypeRepository)
	{
		_map = map;
		_presentationModel = presentationModel;
		_terrainTypeRepository = terrainTypeRepository;

		TerrainType = string.Empty;

		ResourceInfoVM = new ResourceInfoVM(map, presentationModel, resourceTypeRepository);
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
		var terrainType = _terrainTypeRepository.Get(terrainTypeId);
		TerrainType = terrainType.Name;
	}
}



}
