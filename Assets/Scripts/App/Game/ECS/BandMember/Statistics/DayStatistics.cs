using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Gathering.Components;
using App.Game.ECS.BandMember.Movement.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Statistics {



[UpdateInGroup(typeof(DomainSimulation))]
public partial struct DayStatistics : ISystem
{
	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var gameTime = SystemAPI.GetSingleton<GameTime.Components.GameTime>();

		foreach (var (goal, statistics, entity)
		         in SystemAPI.Query<
			         GoalComponent,
			         RefRW<Components.DayStatistics>
			         >()
			         .WithAll<Activity>()
			         .WithEntityAccess())
		{
			if (goal.Goal == Goal.Forage)
				statistics.ValueRW.ForagingHours += gameTime.DeltaHours;

			if (SystemAPI.IsComponentEnabled<GatheringActivity>(entity))
				statistics.ValueRW.GatheringHours += gameTime.DeltaHours;

			if (SystemAPI.IsComponentEnabled<MovementActivity>(entity))
				statistics.ValueRW.MovingHours += gameTime.DeltaHours;

			if (SystemAPI.IsComponentEnabled<LeisureActivity>(entity))
				statistics.ValueRW.LeisureHours += gameTime.DeltaHours;

			if (SystemAPI.IsComponentEnabled<SleepingActivity>(entity))
				statistics.ValueRW.SleepingHours += gameTime.DeltaHours;
		}
	}
}



}
