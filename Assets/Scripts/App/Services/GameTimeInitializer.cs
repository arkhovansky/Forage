using Unity.Entities;

using App.Game;
using App.Game.ECS.Components.Singletons;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.Util.Components;



namespace App.Services {



public class GameTimeInitializer : IGameTimeInitializer
{
	public void Init(YearPeriod yearPeriod)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entityQuery = entityManager.CreateEntityQuery(typeof(SingletonEntity_Tag));
		var entity = entityQuery.GetSingletonEntity();

		entityManager.SetComponentData(entity, new CurrentYearPeriod {Value = yearPeriod});

		entityManager.AddComponentData(entity, new Initialization_Tag());
	}
}



}
