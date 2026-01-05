using Unity.Entities;

using App.Game.Database;
using App.Game.ECS.BandMember.General.Components;
using App.Game.ECS.Map.Components;



namespace App.Game.ECS.BandMember.General.Initialization {



public static class General_HumanInitializer
{
	public static void Initialize(Entity entity, HumanTypeId humanTypeId)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.AddComponentData(entity, new Human(humanTypeId));

		em.AddComponent<Components.BandMember>(entity);

		em.AddComponent<MapPosition>(entity);

		em.AddComponent<Activity>(entity);
		em.SetComponentEnabled<Activity>(entity, false);

		em.AddComponent<LeisureActivity>(entity);
		em.SetComponentEnabled<LeisureActivity>(entity, false);

		em.AddComponent<SleepingActivity>(entity);
		em.SetComponentEnabled<SleepingActivity>(entity, false);
	}
}



}
