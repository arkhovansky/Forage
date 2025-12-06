using System.Collections.Generic;

using Lib.Grid;

using App.Game.Database;
using App.Game.ECS.GameTime.Components;



namespace App.Game.Meta {



public interface ILocale
{
	RectangularHexMap Map { get; }

	float TilePhysicalInnerDiameter { get; }

	IReadOnlyList<TerrainTypeId> TileTerrainTypes { get; }

	IReadOnlyList<ResourceTypeId> ResourceTypes { get; }
	IReadOnlyList<AxialPosition> ResourceAxialPositions { get; }
	IReadOnlyList<float> PotentialBiomass { get; }

	ISet<ResourceTypeId> ResourceTypeIds { get; }

	YearPeriod StartYearPeriod { get; }

	IDictionary<HumanTypeId, uint> HumanTypeCounts { get; }
}



}
