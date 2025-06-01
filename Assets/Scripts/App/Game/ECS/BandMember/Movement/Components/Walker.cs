using Unity.Entities;



namespace App.Game.ECS.BandMember.Movement.Components {



public readonly struct Walker : IComponentData
{
	public readonly float BaseSpeed_KmPerH;


	public Walker(float baseSpeed)
	{
		BaseSpeed_KmPerH = baseSpeed;
	}
}



}
