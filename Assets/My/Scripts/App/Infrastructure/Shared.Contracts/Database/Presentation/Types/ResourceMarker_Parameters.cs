namespace App.Infrastructure.Shared.Contracts.Database.Presentation.Types {



/// <summary>
/// Parameters of a resource marker.
/// </summary>
/// <param name="AreaCoefficient">Coefficient to derive core area from magnitude.</param>
/// <param name="MinCoreRadius">Minimal radius of the core, logical (panel) px.</param>
/// <param name="BorderToCoreRatio">Ratio of border width to core radius.</param>
public record ResourceMarker_Parameters(
	float AreaCoefficient,
	int MinCoreRadius,
	float BorderToCoreRatio
);



}
