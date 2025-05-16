using System.Collections.Generic;

using Lib.Grid;



namespace App.Game.Meta {



public interface IScene
{
	HexGrid Grid { get; }

	IReadOnlyList<uint> TileTerrainTypes { get; }
	IReadOnlyList<AxialPosition> TileAxialPositions { get; }

	IReadOnlyList<uint> ResourceTypes { get; }
	IReadOnlyList<AxialPosition> ResourceAxialPositions { get; }
	IReadOnlyList<float> PotentialBiomass { get; }

	YearPeriod StartYearPeriod { get; }
}



}
