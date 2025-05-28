using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.BandMember.Components {



public struct PathTile : IBufferElementData
{
	public AxialPosition Position;


	public PathTile(AxialPosition position)
	{
		Position = position;
	}
}



}
