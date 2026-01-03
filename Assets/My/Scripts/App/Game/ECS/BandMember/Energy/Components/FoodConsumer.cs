using Unity.Entities;
using UnityEngine;



namespace App.Game.ECS.BandMember.Energy.Components {



public struct FoodConsumer : IComponentData
{
	public readonly uint EnergyRequiredDaily;

	[SerializeField] private float _energyConsumedToday;


	public float EnergyConsumedToday {
		readonly get => _energyConsumedToday;
		private set => _energyConsumedToday = value;
	}

	public readonly float EnergyStillNeeded
		=> EnergyConsumedToday < EnergyRequiredDaily ? EnergyRequiredDaily - EnergyConsumedToday : 0f;

	public readonly bool IsSatiated
		=> EnergyConsumedToday >= EnergyRequiredDaily;



	public FoodConsumer(uint energyRequiredDaily)
	{
		EnergyRequiredDaily = energyRequiredDaily;
		_energyConsumedToday = 0f;
	}


	public void ConsumeEnergy(float energy)
	{
		EnergyConsumedToday += energy;

		if (Mathf.Approximately(EnergyConsumedToday, EnergyRequiredDaily))
			EnergyConsumedToday = EnergyRequiredDaily;
	}


	public void Reset()
		=> _energyConsumedToday = 0f;
}



}
