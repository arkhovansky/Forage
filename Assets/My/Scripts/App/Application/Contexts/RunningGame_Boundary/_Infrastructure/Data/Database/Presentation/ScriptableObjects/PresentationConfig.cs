using System;

using UnityEngine;

using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(PresentationConfig),
                 menuName = AssetMenuNames.Root_Database_GamePresentation_+nameof(PresentationConfig),
                 order = 10)]
public class PresentationConfig : ScriptableObject
{
	public Material TerrainGridLinesMaterial = null!;


	[Serializable]
	public class PlantResourceIcons_Data
	{
		[Tooltip("Biomass per resource icon, kg")]
		public uint BiomassPerIcon = 20;

		[Tooltip("Icon side length relative to inner cell diameter")]
		[Range(0, 1)]
		public float RelativeIconSize = 0.2f;
	}

	public PlantResourceIcons_Data PlantResourceIcons = null!;


	[Serializable]
	public class ResourceMarker_Parameters
	{
		[Tooltip("Coefficient to derive core area from magnitude")]
		[Min(0)]
		public float AreaCoefficient = 5f;

		[Tooltip("Minimal radius of the core, logical (panel) px")]
		[Min(1)]
		public int MinCoreRadius = 10;

		[Tooltip("Ratio of border width to core radius")]
		[Range(0, 1)]
		public float BorderToCoreRatio = 0.2f;
	}

	[Serializable]
	public class ResourceMarker_Config
	{
		public GameObject Prefab = null!;

		public ResourceMarker_Parameters Parameters = null!;
	}

	public ResourceMarker_Config ResourceMarkers = null!;
}



}
