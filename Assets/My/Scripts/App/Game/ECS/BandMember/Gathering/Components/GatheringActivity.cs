using Unity.Entities;



namespace App.Game.ECS.BandMember.Gathering.Components {



public struct GatheringActivity : IComponentData, IEnableableComponent
{
	public Entity ResourceEntity;


	public GatheringActivity(Entity resourceEntity)
	{
		ResourceEntity = resourceEntity;
	}
}



}
