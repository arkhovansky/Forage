using UniMob;

using Lib.Grid;

using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationModel {



/// <summary>
/// Model containing semantic data for a resource marker.
/// </summary>
public interface IResourceMarker_PresentationModel
{
	// Constant

	ResourceTypeId ResourceType { get; }

	AxialPosition TilePosition { get; }


	// Variable

	/// <summary>
	/// Marker magnitude.
	/// </summary>
	/// <remarks>
	/// This value is derived from the resource quantity by an unspecified law.
	/// </remarks>
	Atom<float> Magnitude_Atom { get; }
}



}
