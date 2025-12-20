using Lib.Grid;
using Lib.Grid.Spatial;

using App.Game.ECS.Map.Components.Singletons;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



public class MapDataInitializer : IMapDataInitializer
{
	private readonly HexGridLayout_3D _gridLayout;
	private readonly IEcsHelper _ecsHelper;



	public MapDataInitializer(HexGridLayout_3D gridLayout,
	                          IEcsHelper ecsHelper)
	{
		_gridLayout = gridLayout;
		_ecsHelper = ecsHelper;
	}


	public void Init(RectangularHexMap map)
	{
		_ecsHelper.AddSingletonComponent(new Map(map));
		_ecsHelper.AddSingletonComponent(new HexGridLayout_3D_Component(_gridLayout));
	}
}



}
