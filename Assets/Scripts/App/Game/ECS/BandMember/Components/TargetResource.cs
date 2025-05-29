using Unity.Entities;



namespace App.Game.ECS.BandMember.Components {



public struct TargetResource : IComponentData, IEnableableComponent
{
	public Entity Entity;


	public TargetResource(Entity entity)
	{
		Entity = entity;
	}
}



}
