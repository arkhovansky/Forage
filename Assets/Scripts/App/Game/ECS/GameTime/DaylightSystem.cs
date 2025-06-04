using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.Util.Components;



namespace App.Game.ECS.GameTime {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct DaylightSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<SingletonEntity_Tag>();

		var gameTime = SystemAPI.GetSingleton<Components.GameTime>();

		if (gameTime.IntegerHoursChanged) {
			if (gameTime.Hours > 21)
				state.EntityManager.RemoveComponent<Daylight>(singletonEntity);
			else if (gameTime.Hours > 5)
				state.EntityManager.AddComponent<Daylight>(singletonEntity);
		}
	}
}



}
