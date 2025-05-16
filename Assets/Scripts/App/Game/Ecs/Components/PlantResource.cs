using Unity.Entities;



namespace App.Game.Ecs.Components {



public struct PlantResource : IComponentData
{
	public uint TypeId;

	public YearPeriod RipenessPeriod;

	public float PotentialBiomass;
}



public struct RemainingRipeBiomass : IComponentData
{
	public float Value;
}



}
