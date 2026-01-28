using UnityEngine;

using UniMob;

using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel {



/// <summary>
/// Model containing visual data for a resource marker.
/// </summary>
public interface IResourceMarker_RenderModel
{
	// Constant

	ResourceTypeId ResourceType { get; }

	/// <summary>
	/// World position of the anchor point (bottom center).
	/// </summary>
	Vector3 WorldPosition { get; }

	/// <summary>
	/// Sorting order of the marker; higher is on top.
	/// </summary>
	float Order { get; }


	// Variable

	// Marker is circular and consists of core and border.
	// Core area reflects available quantity of resource.
	// Border is cosmetic.
	// Core radius has a minimum. When available quantity falls below minimum, a ring between smaller "available"
	// circle and larger core ("unavailable" ring) is drawn.

	Atom<float> CoreRadius_Atom { get; }

	Atom<float> UnavailableRingWidth_Atom { get; }

	Atom<float> BorderWidth_Atom { get; }
}



}
