using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember {



[UpdateInGroup(typeof(HumanAI))]
[UpdateAfter(typeof(HumanAI_ForagingTileSelector))]
public partial struct HumanAI_ActivitySelector : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (targetResource, entity)
		         in SystemAPI.Query<
			         TargetResource
			         >()
			         .WithNone<GatheringActivity>()
			         .WithEntityAccess())
		{
			if (state.EntityManager.IsComponentEnabled<TargetTile>(entity)) {
				//TODO
			}
			else {  // Already on resource's tile
				// If ripe biomass has disappeared, disable TargetResource and quit
				if (state.EntityManager.GetComponentData<RipeBiomass>(targetResource.Entity).IsZero) {
					state.EntityManager.SetComponentEnabled<TargetResource>(entity, false);
					continue;
				}

				// Set GatheringActivity

				state.EntityManager.SetComponentEnabled<Activity>(entity, true);

				state.EntityManager.SetComponentData(entity, new GatheringActivity(targetResource.Entity));
				state.EntityManager.SetComponentEnabled<GatheringActivity>(entity, true);
			}
		}
	}
}



}
