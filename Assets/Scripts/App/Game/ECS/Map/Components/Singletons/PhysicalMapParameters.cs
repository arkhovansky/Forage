﻿using Unity.Entities;

using Lib.VisualGrid;



namespace App.Game.ECS.Map.Components.Singletons {



public readonly struct PhysicalMapParameters : IComponentData
{
	/// <summary>
	/// Inner diameter of a tile in km.
	/// </summary>
	public readonly float TileInnerDiameter;

	public readonly float CellArea;



	public PhysicalMapParameters(float tileInnerDiameter)
	{
		TileInnerDiameter = tileInnerDiameter;
		CellArea = HexLayout.CellArea_From_InnerDiameter(tileInnerDiameter);
	}
}



}
