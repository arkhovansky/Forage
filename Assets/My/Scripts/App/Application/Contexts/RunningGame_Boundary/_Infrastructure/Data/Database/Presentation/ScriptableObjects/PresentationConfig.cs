using System;

using UnityEngine;

using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(PresentationConfig),
                 menuName = AssetMenuNames.Root_Database_GamePresentation_+nameof(PresentationConfig),
                 order = 10)]
public class PresentationConfig : ScriptableObject
{
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
}



}
