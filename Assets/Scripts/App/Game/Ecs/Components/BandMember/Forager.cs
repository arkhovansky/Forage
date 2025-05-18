using Unity.Entities;



namespace App.Game.Ecs.Components.BandMember {



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
