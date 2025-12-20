using Lib.Grid;
using Lib.Grid.Spatial;

using App.Game.ECS.Map.Components.Singletons;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer {



public class MapDataInitializer : IMapDataInitializer
{
	private readonly HexGridLayout_3D _gridLayout;



	public MapDataInitializer(HexGridLayout_3D gridLayout)
	{
		_gridLayout = gridLayout;
	}


	public void Init(RectangularHexMap map)
	{
		EcsService.AddSingletonComponent(new Map(map));
		EcsService.AddSingletonComponent(new HexGridLayout_3D_Component(_gridLayout));
	}
}



}
