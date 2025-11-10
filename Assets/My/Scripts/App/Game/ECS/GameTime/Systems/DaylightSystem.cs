using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components;
using App.Game.ECS.GameTime.Rules;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.Util.Components;



namespace App.Game.ECS.GameTime.Systems {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct DaylightSystem : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<SingletonEntity_Tag>();

		var gameTime = SystemAPI.GetSingleton<Components.GameTime>();

		// This works as long as simulation starts before daylight
		bool daylightChanged = Daylight_Rules.GetDaylightEvent(in gameTime, out bool isDaylight);
		if (daylightChanged) {
			if (isDaylight)
				state.EntityManager.AddComponent<Daylight>(singletonEntity);
			else
				state.EntityManager.RemoveComponent<Daylight>(singletonEntity);
		}
	}
}



}
