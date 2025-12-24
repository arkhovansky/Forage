using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine.Assertions;

using Lib.Grid;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Locale.TileMapStubs;
using App.Game.Database;
using App.Game.ECS.GameTime.Components;
using App.Game.Meta;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Locale {



public class Locale : ILocale
{
	private readonly Locale_Asset _asset;



	public RectangularHexMap Map { get; }

	public float TilePhysicalInnerDiameter
		=> _asset.TilePhysicalInnerDiameter;

	public IReadOnlyList<TerrainTypeId> TileTerrainTypes { get; }

	public IReadOnlyList<ResourceTypeId> ResourceTypes
		=> _asset.PlantResourcePatches.Select(r => r.ResourceTypeId).ToArray();

	public IReadOnlyList<AxialPosition> ResourceAxialPositions
		=> _asset.PlantResourcePatches.Select(r => AxialPosition_From(r.Position)).ToArray();

	public IReadOnlyList<float> PotentialBiomass
		=> _asset.PlantResourcePatches.Select(r => r.Biomass).ToArray();

	public ISet<ResourceTypeId> ResourceTypeIds
		=> _asset.PlantResourcePatches.Select(r => r.ResourceTypeId).ToHashSet();

	public YearPeriod StartYearPeriod
		=> _asset.StartYearPeriod;

	public IDictionary<HumanTypeId, uint> HumanTypeCounts { get; }



	public Locale(Locale_Asset asset)
	{
		_asset = asset;

		Map = new RectangularHexMap(_asset.MapSize.Width, _asset.MapSize.Height,
		                            HexOrientation.FlatTop, HexMapLineOffset.Odd);

		uint tileCount = _asset.MapSize.Width * _asset.MapSize.Height;

		var tiles = TileMapStub_Repository.Get(new LocaleId(_asset.Id));
		TileTerrainTypes = tiles.Cast<TerrainTypeId>().ToArray();

		Assert.AreEqual(TileTerrainTypes.Count, tileCount);


		HumanTypeCounts = new Dictionary<HumanTypeId, uint>();
		foreach (HumanTypeId humanTypeId in Enum.GetValues(typeof(HumanTypeId))) {
			HumanTypeCounts.Add(humanTypeId,
			                    _asset.HumanTypeCounts.First(x => x.HumanTypeId == humanTypeId).Count);
		}
	}


	//----------------------------------------------------------------------------------------------
	// private


	private AxialPosition AxialPosition_From(Locale_Asset.MapPosition mapPosition)
	{
		return Map.AxialPositionFrom(OffsetPosition_From(mapPosition));
	}

	private OffsetPosition OffsetPosition_From(Locale_Asset.MapPosition mapPosition)
	{
		return new OffsetPosition((int)mapPosition.X, (int)mapPosition.Y);
	}
}



}
