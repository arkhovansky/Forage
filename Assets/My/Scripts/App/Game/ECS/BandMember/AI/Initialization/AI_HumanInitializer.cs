using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;



namespace App.Game.ECS.BandMember.AI.Initialization {



public static class AI_HumanInitializer
{
	public static void Initialize(Entity entity)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.AddComponent<GoalComponent>(entity);
		em.SetComponentEnabled<GoalComponent>(entity, false);

		em.AddComponent<Forage_Goal>(entity);
		em.SetComponentEnabled<Forage_Goal>(entity, false);

		em.AddComponent<Task>(entity);
		em.SetComponentEnabled<Task>(entity, false);

		em.AddComponent<ForageOnTile_Task>(entity);
		em.SetComponentEnabled<ForageOnTile_Task>(entity, false);

		em.AddComponent<Leisure_Goal>(entity);
		em.SetComponentEnabled<Leisure_Goal>(entity, false);

		em.AddComponent<Leisure_Task>(entity);
		em.SetComponentEnabled<Leisure_Task>(entity, false);

		em.AddComponent<Sleep_Goal>(entity);
		em.SetComponentEnabled<Sleep_Goal>(entity, false);

		em.AddComponent<Sleep_Task>(entity);
		em.SetComponentEnabled<Sleep_Task>(entity, false);
	}
}



}
