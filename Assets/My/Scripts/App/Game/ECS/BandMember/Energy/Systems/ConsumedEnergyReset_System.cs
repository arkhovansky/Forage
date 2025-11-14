using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Energy.Components;
using App.Game.ECS.GameTime.Components.Events;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Energy.Systems {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct ConsumedEnergyReset_System : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DayChanged>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var foodConsumer
		         in SystemAPI.Query<RefRW<FoodConsumer>>())
		{
			foodConsumer.ValueRW.Reset();
		}
	}
}



}
