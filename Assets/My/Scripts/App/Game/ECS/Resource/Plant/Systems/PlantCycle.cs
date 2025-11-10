using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components.Events;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Resource.Plant.Rules;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.Resource.Plant.Systems {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct PlantCycle : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GameTime.Components.GameTime>();
		state.RequireForUpdate<YearPeriodChanged>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var yearPeriod = SystemAPI.GetSingleton<GameTime.Components.GameTime>().YearPeriod;

		foreach (var (resource, ripeBiomass)
		         in SystemAPI.Query<RefRO<PlantResource>, RefRW<RipeBiomass>>()) {
			PlantCycle_Rules.UpdateRipeBiomass(ref ripeBiomass.ValueRW, resource.ValueRO, yearPeriod);
		}
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }
}



}
