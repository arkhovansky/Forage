using Unity.Entities;

using App.Game.ECS.BandMember.Statistics.Components;



namespace App.Game.ECS.BandMember.Statistics.Initialization {



public static class Statistics_HumanInitializer
{
	public static void Initialize(Entity entity)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.AddComponent<DayStatistics>(entity);
		em.AddComponent<YearPeriodStatistics>(entity);
	}
}



}
