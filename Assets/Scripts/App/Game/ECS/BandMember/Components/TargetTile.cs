using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.BandMember.Components {



public struct TargetTile : IComponentData, IEnableableComponent
{
	public AxialPosition Position;


	public TargetTile(AxialPosition position)
	{
		Position = position;
	}
}



}
