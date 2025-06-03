using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct ConsumedEnergyReset_System : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var gameTime = SystemAPI.GetSingleton<GameTime.Components.GameTime>();

		if (!gameTime.DayChanged)
			return;

		foreach (var foodConsumer
		         in SystemAPI.Query<RefRW<FoodConsumer>>())
		{
			foodConsumer.ValueRW.Reset();
		}
	}
}



}
