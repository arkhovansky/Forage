using System.Collections.Generic;
using System.Linq;

using UnityEngine.Assertions;

using Lib.Grid;

using App.Game.Database;
using App.Game.ECS.GameTime.Components;



namespace App.Game.Meta {



public class Scene_Stub : IScene
{
	public RectangularHexMap Map { get; }

	public float TilePhysicalInnerDiameter { get; }

	public IReadOnlyList<TerrainTypeId> TileTerrainTypes { get; }

	public IReadOnlyList<ResourceTypeId> ResourceTypes {
		get {
			return _resources.Select(r => r.Resource.Type).ToArray();
		}
	}

	public IReadOnlyList<AxialPosition> ResourceAxialPositions {
		get {
			return _resources.Select(r => r.Position).ToArray();
		}
	}

	public IReadOnlyList<float> PotentialBiomass {
		get {
			return _resources.Select(r => r.Resource.Biomass).ToArray();
		}
	}

	public ISet<ResourceTypeId> ResourceTypeIds
		=> _resources.Select(r => r.Resource.Type).ToHashSet();

	public YearPeriod StartYearPeriod { get; }

	public IDictionary<uint, uint> HumanTypeCounts { get; }



	private record Resource(
		ResourceTypeId Type,
		float Biomass);

	private record TileResource(
		Resource Resource,
		AxialPosition Position);



	private readonly uint _width = 12;
	private readonly uint _height = 8;

	private readonly List<TileResource> _resources = new();



	public Scene_Stub()
	{
		Map = new RectangularHexMap(_width, _height, HexOrientation.FlatTop, HexMapLineOffset.Odd);

		TilePhysicalInnerDiameter = 1;

		uint tileCount = _width * _height;


		var tiles = new int[] {
			5, 5, 5, 5, 5, 5, 3, 3, 4, 4, 4, 8,
			5, 5, 5, 5, 5, 0, 3, 4, 4, 4, 7, 8,
			5, 5, 5, 5, 5, 3, 3, 3, 4, 4, 7, 8,
			5, 5, 5, 5, 3, 3, 3, 4, 4, 4, 4, 7,
			6, 5, 5, 3, 3, 3, 3, 4, 4, 4, 7, 7,
			6, 6, 6, 3, 3, 3, 4, 4, 4, 7, 7, 8,
			6, 6, 6, 6, 1, 1, 1, 4, 4, 1, 4, 4,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
		};
		TileTerrainTypes = tiles.Cast<TerrainTypeId>().ToArray();

		Assert.AreEqual(TileTerrainTypes.Count, tileCount);


		for (uint y = 0; y < _height; ++y) {
			for (uint x = 0; x < _width; ++x) {
				var tileIndex = y * _width + x;

				var resource = ResourceForTerrain(TileTerrainTypes[(int) tileIndex]);
				if (resource != null)
					_resources.Add(new TileResource(resource, Map.AxialPositionFromCellIndex(tileIndex)));
			}
		}


		StartYearPeriod = new YearPeriod {Month = Month.June};


		HumanTypeCounts = new Dictionary<uint, uint>();
		HumanTypeCounts.Add((uint)Gender.Male, 5);
		HumanTypeCounts.Add((uint)Gender.Female, 5);
	}


	private static Resource? ResourceForTerrain(TerrainTypeId terrainType)
	{
		const float scale = 1;

		return terrainType switch {
			TerrainTypeId.Grasslands => new Resource(ResourceTypeId.Yam, 300 * scale),
			TerrainTypeId.Plains => new Resource(ResourceTypeId.Wheat, 600 * scale),
			TerrainTypeId.Forest => new Resource(ResourceTypeId.Acorns, 400 * scale),
			TerrainTypeId.TropicalForest => new Resource(ResourceTypeId.Bananas, 500 * scale),
			_ => null
		};
	}
}



}
