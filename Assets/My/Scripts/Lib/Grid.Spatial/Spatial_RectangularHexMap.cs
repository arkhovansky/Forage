using System;

using UnityEngine;



namespace Lib.Grid.Spatial {



/// <summary>
/// Hex grid with specific dimensions and geometry layout.
/// </summary>
public class Spatial_RectangularHexMap
{
	public RectangularHexMap Map { get; }

	public HexGridLayout Layout { get; }


	//----------------------------------------------------------------------------------------------


	public Spatial_RectangularHexMap(RectangularHexMap map, HexGridLayout layout)
	{
		if (layout.Orientation != map.Orientation)
			throw new ArgumentException();

		Map = map;
		Layout = layout;
	}


	public AxialPosition? GetAxialPosition(Vector2 point)
	{
		AxialPosition freePos = Layout.GetAxialPosition(point);
		return Map.Contains(freePos) ? freePos : null;
	}
}



}
