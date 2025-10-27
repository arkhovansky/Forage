using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.Map.Components.Singletons {



public struct Map : IComponentData
{
	public RectangularHexMap Value;



	public Map(RectangularHexMap value)
	{
		Value = value;
	}

	public static implicit operator RectangularHexMap(Map map)
		=> map.Value;
}



}
