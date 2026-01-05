using Unity.Entities;



namespace App.Game.ECS.BandMember.Gathering.Components {



public readonly struct Gatherer : IComponentData
{
	public readonly float GatheringSpeed;


	public Gatherer(float gatheringSpeed)
	{
		GatheringSpeed = gatheringSpeed;
	}
}



}
