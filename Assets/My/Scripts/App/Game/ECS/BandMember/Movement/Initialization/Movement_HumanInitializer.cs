using Unity.Entities;

using App.Game.ECS.BandMember.Movement.Components;



namespace App.Game.ECS.BandMember.Movement.Initialization {



public static class Movement_HumanInitializer
{
	public static void Initialize(Entity entity, float baseSpeed)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.AddComponentData(entity, new Walker(baseSpeed));

		em.AddBuffer<PathTile>(entity);

		em.AddComponent<IntraCellMovement>(entity);

		em.AddComponent<MovementActivity>(entity);
		em.SetComponentEnabled<MovementActivity>(entity, false);
	}
}



}
