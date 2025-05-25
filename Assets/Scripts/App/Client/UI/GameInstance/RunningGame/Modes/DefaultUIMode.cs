using Unity.Entities;

using Lib.Grid;

using App.Game.ECS.UI.HoveredTile.Components;
using App.Game.ECS.Util.Components;



namespace App.Client.UI.GameInstance.RunningGame {



public class DefaultUIMode : IUIMode
{
	public void Update(AxialPosition? oldHoveredTilePosition, AxialPosition? newHoveredTilePosition)
	{
		if (newHoveredTilePosition != oldHoveredTilePosition)
			NotifySystems_HoveredTileChanged(newHoveredTilePosition);
	}


	private void NotifySystems_HoveredTileChanged(AxialPosition? tilePosition)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var entityQuery = entityManager.CreateEntityQuery(typeof(SingletonEntity_Tag));
		var singletonEntity = entityQuery.GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, new HoveredTileChanged_Event(tilePosition));
	}
}



}
