using Unity.Entities;

using App.Game.ECS.GameTime.Components;



namespace App.Game.ECS.Resource.Plant.Components {



public struct PlantResource : IComponentData
{
	public uint TypeId;

	public YearPeriod RipenessPeriod;

	public float PotentialBiomass;
}



}
