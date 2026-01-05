using Unity.Entities;

using App.Game.ECS.BandMember.Gathering.Components;



namespace App.Game.ECS.BandMember.Gathering.Initialization {



public static class Gathering_HumanInitializer
{
	public static void Initialize(Entity entity, float gatheringSpeed)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.AddComponentData(entity, new Gatherer(gatheringSpeed));

		em.AddComponent<GatheringActivity>(entity);
		em.SetComponentEnabled<GatheringActivity>(entity, false);
	}
}



}
