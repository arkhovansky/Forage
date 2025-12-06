using System;
using System.Collections.Generic;

using UnityEngine;

using App.Game.Database;
using App.Game.ECS.GameTime.Components;



namespace App.Infrastructure.External.Data.Locale {



[CreateAssetMenu(fileName = "Locale",
                 menuName = AssetMenuNames.Root_+"Locale")]
public class Locale_Asset : ScriptableObject
{
	public string Id = null!;


	[Serializable]
	public struct RectSize {
		public uint Width;
		public uint Height;
	}

	public RectSize MapSize;


	[Tooltip("Physical inner diameter of a tile, km")]
	public float TilePhysicalInnerDiameter;


	[Serializable]
	public struct MapPosition {
		public uint X;
		public uint Y;
	}

	[Serializable]
	public class PlantResourcePatch {
		public MapPosition Position;
		public ResourceTypeId ResourceTypeId;
		public float Biomass;
	}

	public List<PlantResourcePatch> PlantResourcePatches = null!;


	public YearPeriod StartYearPeriod;


	[Serializable]
	public class HumanTypeCount
	{
		public HumanTypeId HumanTypeId;
		public uint Count;
	}

	public List<HumanTypeCount> HumanTypeCounts = null!;
}



}
