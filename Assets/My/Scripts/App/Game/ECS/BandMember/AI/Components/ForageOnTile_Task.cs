using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.BandMember.AI.Components {



public struct ForageOnTile_Task : IComponentData, IEnableableComponent
{
	public AxialPosition Position;

	public Entity TargetResourceEntity;


	public ForageOnTile_Task(AxialPosition position, Entity targetResourceEntity)
	{
		Position = position;
		TargetResourceEntity = targetResourceEntity;
	}
}



}
