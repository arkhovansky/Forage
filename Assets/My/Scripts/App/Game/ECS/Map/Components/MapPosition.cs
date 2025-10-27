using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.Map.Components {



public struct MapPosition : IComponentData
{
	public AxialPosition Value;



	public MapPosition(AxialPosition value)
	{
		Value = value;
	}


	public static implicit operator AxialPosition(MapPosition position)
		=> position.Value;
}



}
