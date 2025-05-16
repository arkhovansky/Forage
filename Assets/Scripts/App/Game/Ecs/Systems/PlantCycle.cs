using Unity.Burst;
using Unity.Entities;

using App.Game.Ecs.Components;
using App.Game.Ecs.Components.Singletons.YearPeriod;



namespace App.Game.Ecs.Systems {



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
		         in SystemAPI.Query<RefRO<PlantResource>, RefRW<RemainingRipeBiomass>>()) {
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
