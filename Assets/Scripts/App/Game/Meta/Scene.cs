using System.Collections.Generic;

using App.Services.BandMembers;

using Lib.Grid;



namespace App.Game.Meta {



public class Scene : IScene
{
	public RectangularHexMap Map { get; }

	public IReadOnlyList<uint> TileTerrainTypes { get; }
	public IReadOnlyList<AxialPosition> TileAxialPositions => _tileAxialPositions;

	public IReadOnlyList<uint> ResourceTypes { get; }
	public IReadOnlyList<AxialPosition> ResourceAxialPositions { get; }
	public IReadOnlyList<float> PotentialBiomass { get; }

	public YearPeriod StartYearPeriod { get; }

	public IDictionary<uint, uint> BandMemberTypeCounts { get; }



	private readonly uint _width = 12;
	private readonly uint _height = 8;

	private readonly AxialPosition[] _tileAxialPositions;



	public Scene()
	{
		Map = new RectangularHexMap(_width, _height, HexOrientation.FlatTop, HexMapLineOffset.Odd);

		uint tileCount = _width * _height;


		TileTerrainTypes = new uint[] {
			5, 5, 5, 5, 5, 5, 3, 3, 4, 4, 4, 7,
			5, 5, 5, 5, 5, 0, 3, 4, 4, 4, 6, 7,
			5, 5, 5, 5, 5, 3, 3, 3, 4, 4, 6, 7,
			5, 5, 5, 5, 3, 3, 3, 4, 4, 4, 4, 6,
			5, 5, 5, 3, 3, 3, 3, 4, 4, 4, 6, 6,
			5, 5, 5, 3, 3, 3, 4, 4, 4, 6, 6, 7,
			5, 5, 5, 5, 1, 1, 1, 4, 4, 1, 4, 4,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
		};


		_tileAxialPositions = new AxialPosition[tileCount];

		for (uint y = 0; y < _height; ++y) {
			for (uint x = 0; x < _width; ++x) {
				var tileIndex = y * _width + x;

				_tileAxialPositions[tileIndex] = Map.AxialPositionFromCellIndex(tileIndex);
			}
		}



		const int resourceCount = 9;

		ResourceTypes = new uint[resourceCount] {
			0,
			0,
			0,
			1,
			1,
			2,
			3,
			3,
			3
		};

		ResourceAxialPositions = new AxialPosition[resourceCount] {
			Map.AxialPositionFrom(new OffsetPosition(0, 0)),
			Map.AxialPositionFrom(new OffsetPosition(2, 1)),
			Map.AxialPositionFrom(new OffsetPosition(1, 3)),

			Map.AxialPositionFrom(new OffsetPosition(1, 6)),
			Map.AxialPositionFrom(new OffsetPosition(3, 6)),

			Map.AxialPositionFrom(new OffsetPosition(1, 5)),

			Map.AxialPositionFrom(new OffsetPosition(9, 1)),
			Map.AxialPositionFrom(new OffsetPosition(8, 3)),
			Map.AxialPositionFrom(new OffsetPosition(7, 5))
		};

		PotentialBiomass = new float[resourceCount] {
			1000,
			300,
			500,

			700,
			600,

			800,

			800,
			600,
			400
		};



		// _resourceInTilePositions = new Vector2[resourceCount] {
		// 	new (0, 0)
		// };



		StartYearPeriod = new YearPeriod {Month = Month.June};



		BandMemberTypeCounts = new Dictionary<uint, uint>();
		BandMemberTypeCounts.Add((uint)Gender.Male, 2);
		BandMemberTypeCounts.Add((uint)Gender.Female, 2);
	}
}



}
