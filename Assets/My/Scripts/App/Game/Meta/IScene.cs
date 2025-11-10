using System.Collections.Generic;

using Lib.Grid;

using App.Game.ECS.GameTime.Components;



namespace App.Game.Meta {



public interface IScene
{
	RectangularHexMap Map { get; }

	float TilePhysicalInnerDiameter { get; }

	IReadOnlyList<uint> TileTerrainTypes { get; }

	IReadOnlyList<uint> ResourceTypes { get; }
	IReadOnlyList<AxialPosition> ResourceAxialPositions { get; }
	IReadOnlyList<float> PotentialBiomass { get; }

	ISet<uint> ResourceTypeIds { get; }

	YearPeriod StartYearPeriod { get; }

	IDictionary<uint, uint> BandMemberTypeCounts { get; }
}



}
