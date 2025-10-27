using Unity.Entities;

using Lib.Grid;



namespace App.Game.ECS.BandMember.Movement.Components {



public struct MovementActivity : IComponentData, IEnableableComponent
{
	public AxialPosition TargetPosition;


	public MovementActivity(AxialPosition targetPosition)
	{
		TargetPosition = targetPosition;
	}
}



}
