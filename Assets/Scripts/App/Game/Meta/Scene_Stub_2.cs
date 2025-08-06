using System.Collections.Generic;
using System.Linq;

using UnityEngine.Assertions;

using Lib.Grid;

using App.Services.BandMembers;



namespace App.Game.Meta {



public class Scene_Stub_2 : IScene
{
	public RectangularHexMap Map { get; }

	public float TilePhysicalInnerDiameter { get; }

	public IReadOnlyList<uint> TileTerrainTypes { get; }

	public IReadOnlyList<uint> ResourceTypes {
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

	public ISet<uint> ResourceTypeIds
		=> _resources.Select(r => r.Resource.Type).ToHashSet();

	public YearPeriod StartYearPeriod { get; }

	public IDictionary<uint, uint> BandMemberTypeCounts { get; }



	private record Resource(
		uint Type,
		float Biomass);

	private record TileResource(
		Resource Resource,
		AxialPosition Position);



	private readonly uint _width = 12;
	private readonly uint _height = 8;

	private readonly List<TileResource> _resources = new();



	public Scene_Stub_2()
	{
		Map = new RectangularHexMap(_width, _height, HexOrientation.FlatTop, HexMapLineOffset.Odd);

		TilePhysicalInnerDiameter = 2;

		uint tileCount = _width * _height;


		TileTerrainTypes = new uint[] {
			6, 6, 6, 6, 6, 6, 6, 3, 4, 4, 4, 8,
			6, 6, 6, 6, 6, 6, 6, 4, 4, 4, 7, 8,
			6, 6, 6, 6, 6, 6, 6, 3, 4, 4, 7, 8,
			6, 6, 6, 6, 6, 6, 6, 4, 4, 4, 4, 7,
			6, 6, 6, 6, 6, 6, 6, 4, 4, 4, 7, 7,
			6, 6, 6, 6, 6, 6, 6, 4, 4, 7, 7, 8,
			6, 6, 6, 6, 6, 6, 6, 4, 4, 1, 4, 4,
			6, 6, 6, 6, 6, 6, 6, 1, 1, 1, 1, 1
		};

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


		BandMemberTypeCounts = new Dictionary<uint, uint>();
		BandMemberTypeCounts.Add((uint)Gender.Male, 15);
		BandMemberTypeCounts.Add((uint)Gender.Female, 15);
	}


	private static Resource? ResourceForTerrain(uint terrainType)
	{
		const float scale = 1;

		return terrainType switch {
			3 => new Resource(2, 200 * scale),
			4 => new Resource(3, 500 * scale),
			// 5 => new Resource(0, 300 * scale),
			6 => new Resource(1, 200 * scale),
			_ => null
		};
	}
}



}
