using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.Resource.Plant {



[UpdateInGroup(typeof(DiscreteActions))]
[UpdateAfter(typeof(GameTimeSystem))]
public partial struct PlantCycle : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<CurrentYearPeriod>();
		state.RequireForUpdate<YearPeriodChanged_Event>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var currentYearPeriod = SystemAPI.GetSingleton<CurrentYearPeriod>();

		foreach (var (resource, ripeBiomass)
		         in SystemAPI.Query<RefRO<PlantResource>, RefRW<RipeBiomass>>()) {
			if (resource.ValueRO.RipenessPeriod == currentYearPeriod.Value)
				ripeBiomass.ValueRW.Value = resource.ValueRO.PotentialBiomass;
			else
				ripeBiomass.ValueRW.Value = 0;
		}
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }
}



}
