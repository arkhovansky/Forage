using Unity.Entities;

using Lib.Grid;

using App.Game.ECS.Map.Components.Singletons;



namespace App.Game.ECS.Map {



public struct EcsMap
{
	private readonly RectangularHexMap _map;
	private readonly DynamicBuffer<MapTileEntity> _tileIndexBuffer;



	public EcsMap(RectangularHexMap map, DynamicBuffer<MapTileEntity> tileIndexBuffer)
	{
		_map = map;
		_tileIndexBuffer = tileIndexBuffer;
	}


	public readonly Entity GetTileEntity(AxialPosition position)
	{
		return GetTileEntity(position, _map, _tileIndexBuffer);
	}


	public static Entity GetTileEntity(AxialPosition position,
	                                   in RectangularHexMap map, in DynamicBuffer<MapTileEntity> tileIndexBuffer)
	{
		int cellIndex = (int) map.CellIndexFrom(position);
		return tileIndexBuffer[cellIndex];
	}
}



}
