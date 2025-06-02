using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.Resource.Plant {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct PlantCycle : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<GameTime.Components.GameTime>();
		state.RequireForUpdate<YearPeriodChanged_Event>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var yearPeriod = SystemAPI.GetSingleton<GameTime.Components.GameTime>().YearPeriod;

		foreach (var (resource, ripeBiomass)
		         in SystemAPI.Query<RefRO<PlantResource>, RefRW<RipeBiomass>>()) {
			if (resource.ValueRO.RipenessPeriod == yearPeriod)
				ripeBiomass.ValueRW.Reset(resource.ValueRO.PotentialBiomass);
			else
				ripeBiomass.ValueRW.Reset(0);
		}
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state) { }
}



}
