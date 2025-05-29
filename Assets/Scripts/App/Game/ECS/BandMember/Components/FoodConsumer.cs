using Unity.Entities;



namespace App.Game.ECS.BandMember.Components {



public struct FoodConsumer : IComponentData
{
	public readonly uint EnergyNeededPerDay;

	public uint EnergyConsumedToday;


	public readonly uint EnergyStillNeeded
		=> EnergyConsumedToday < EnergyNeededPerDay ? EnergyNeededPerDay - EnergyConsumedToday : 0;

	public readonly bool IsSatiated
		=> EnergyConsumedToday >= EnergyNeededPerDay;



	public FoodConsumer(uint energyNeededPerDay)
	{
		EnergyNeededPerDay = energyNeededPerDay;
		EnergyConsumedToday = 0;
	}
}



}
