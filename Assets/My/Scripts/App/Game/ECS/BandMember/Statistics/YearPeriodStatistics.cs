using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.GameTime.Components.Events;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember.Statistics {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct YearPeriodStatistics : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DayChanged>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (dayStatistics, yearPeriodStatistics)
		         in SystemAPI.Query<
			         RefRW<Components.DayStatistics>,
			         RefRW<Components.YearPeriodStatistics>
			         >())
		{
			yearPeriodStatistics.ValueRW.AddDayStatistics(in dayStatistics.ValueRO);

			dayStatistics.ValueRW.Reset();
		}
	}
}



}
