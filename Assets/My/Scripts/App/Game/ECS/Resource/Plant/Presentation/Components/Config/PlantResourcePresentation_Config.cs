using Unity.Entities;



namespace App.Game.ECS.Resource.Plant.Presentation.Components.Config {



public struct PlantResourcePresentation_Config : IComponentData
{
	/// <summary>
	/// Biomass per resource icon, kg
	/// </summary>
	public readonly uint BiomassPerIcon;

	/// <summary>
	/// Icon side length relative to inner cell diameter
	/// </summary>
	public readonly float RelativeIconSize;



	public PlantResourcePresentation_Config(
		uint biomassPerIcon,
		float relativeIconSize)
	{
		BiomassPerIcon = biomassPerIcon;
		RelativeIconSize = relativeIconSize;
	}
}



}
