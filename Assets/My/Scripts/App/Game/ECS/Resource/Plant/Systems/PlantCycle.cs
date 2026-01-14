using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components.Events;
using App.Game.ECS.Resource.Plant.Components;
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

		var ecbs = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
		var ecb = ecbs.CreateCommandBuffer(state.WorldUnmanaged);

		foreach (var (resource, resourceEntity)
		         in SystemAPI.Query<RefRO<PlantResource>>().WithEntityAccess())
		{
			if (resource.ValueRO.RipenessPeriod == yearPeriod) {
				ecb.AddComponent<RipeBiomass>(resourceEntity);
				ecb.SetComponent(resourceEntity, new RipeBiomass(resource.ValueRO.PotentialBiomass));
			}
			else
				ecb.RemoveComponent<RipeBiomass>(resourceEntity);
		}
	}
}



}
