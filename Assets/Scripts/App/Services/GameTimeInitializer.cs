using Unity.Entities;

using App.Game;
using App.Game.ECS.GameTime.Components.Commands;
using App.Game.ECS.Util.Components;



namespace App.Services {



public class GameTimeInitializer : IGameTimeInitializer
{
	public void Init(YearPeriod yearPeriod)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entityQuery = entityManager.CreateEntityQuery(typeof(SingletonEntity_Tag));
		var entity = entityQuery.GetSingletonEntity();

		entityManager.AddComponentData(entity, new InitYearPeriod(yearPeriod));
	}
}



}
