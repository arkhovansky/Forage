using Unity.Entities;

using App.Game.ECS.BandMember.Energy.Components;



namespace App.Game.ECS.BandMember.Energy.Initialization {



public static class Energy_HumanInitializer
{
	public static void Initialize(Entity entity, uint energyRequiredDaily)
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		em.AddComponentData(entity, new FoodConsumer(energyRequiredDaily));
	}
}



}
