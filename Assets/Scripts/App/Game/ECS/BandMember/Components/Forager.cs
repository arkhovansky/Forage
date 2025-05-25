using Unity.Entities;



namespace App.Game.ECS.BandMember.Components {



public enum Activity
{
	Idle,
	Gathering
}


public struct Forager : IComponentData
{
	public float GatheringSpeed;

	public Activity Activity;
}



}
