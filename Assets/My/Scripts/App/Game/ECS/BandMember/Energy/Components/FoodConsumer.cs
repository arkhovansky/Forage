using Unity.Entities;
using UnityEngine;



namespace App.Game.ECS.BandMember.Energy.Components {



public struct FoodConsumer : IComponentData
{
	public readonly uint EnergyNeededPerDay;

	[SerializeField] private float _energyConsumedToday;


	public float EnergyConsumedToday {
		readonly get => _energyConsumedToday;
		private set => _energyConsumedToday = value;
	}

	public readonly float EnergyStillNeeded
		=> EnergyConsumedToday < EnergyNeededPerDay ? EnergyNeededPerDay - EnergyConsumedToday : 0f;

	public readonly bool IsSatiated
		=> EnergyConsumedToday >= EnergyNeededPerDay;



	public FoodConsumer(uint energyNeededPerDay)
	{
		EnergyNeededPerDay = energyNeededPerDay;
		_energyConsumedToday = 0f;
	}


	public void ConsumeEnergy(float energy)
	{
		EnergyConsumedToday += energy;

		if (Mathf.Approximately(EnergyConsumedToday, EnergyNeededPerDay))
			EnergyConsumedToday = EnergyNeededPerDay;
	}


	public void Reset()
		=> _energyConsumedToday = 0f;
}



}
