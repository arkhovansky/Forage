﻿using Lib.Grid;

using Unity.Entities;



namespace App.Game.ECS.UI.HoveredTile.Components {



public struct HoveredTileChanged_Event : IComponentData
{
	public AxialPosition? NewPosition;


	public HoveredTileChanged_Event(AxialPosition? newPosition)
	{
		NewPosition = newPosition;
	}
}



}
