using UnityEngine;

using UniMob;
using AtomLifetime = UniMob.Lifetime;

using Lib.Grid.Spatial;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationModel;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.Shared;
using App.Game.Database;
using App.Infrastructure.Shared.Contracts.Database.Presentation.Types;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel.Impl {



public class ResourceMarker_RenderModel
	: IResourceMarker_RenderModel,
	  IResettable
{
	private readonly IResourceMarker_PresentationModel _presentationModel;

	private readonly ResourceMarker_Parameters _parameters;

	private readonly HexGridLayout_3D _gridLayout;

	//----------------------------------------------------------------------------------------------


	public ResourceMarker_RenderModel(
		IResourceMarker_PresentationModel presentationModel,
		ResourceMarker_Parameters parameters,
		in HexGridLayout_3D gridLayout,
		AtomLifetime atomLifetime)
	{
		_presentationModel = presentationModel;
		_parameters = parameters;
		_gridLayout = gridLayout;

		var availableCircleRadius =
			Atom.Computed(atomLifetime, () =>
				              Compute_AvailableCircleRadius(_presentationModel.Magnitude_Atom.Value),
			              false, "ResourceMarker_RenderModel: availableCircleRadius");

		CoreRadius_Atom =
			Atom.Computed(atomLifetime, () =>
				              Compute_CoreRadius(availableCircleRadius.Value),
			              false, "ResourceMarker_RenderModel: CoreRadius");
		UnavailableRingWidth_Atom =
			Atom.Computed(atomLifetime, () =>
				              Compute_UnavailableRingWidth(availableCircleRadius.Value),
			              false, "ResourceMarker_RenderModel: UnavailableRingWidth");
		BorderWidth_Atom =
			Atom.Computed(atomLifetime, () =>
				              Compute_BorderWidth(CoreRadius_Atom.Value),
			              false, "ResourceMarker_RenderModel: BorderWidth");
	}


	//----------------------------------------------------------------------------------------------
	// IResettable


	public void Reset()
	{
		ResourceType = _presentationModel.ResourceType;
		WorldPosition = _gridLayout.GetPoint(_presentationModel.TilePosition);
		Order = -WorldPosition.z;
	}


	//----------------------------------------------------------------------------------------------
	// IResourceMarker_RenderModel


	public ResourceTypeId ResourceType { get; private set; }

	public Vector3 WorldPosition { get; private set; }

	public float Order { get; private set; }


	public Atom<float> CoreRadius_Atom { get; }

	public Atom<float> UnavailableRingWidth_Atom { get; }

	public Atom<float> BorderWidth_Atom { get; }


	//----------------------------------------------------------------------------------------------
	// private


	private float Compute_AvailableCircleRadius(float magnitude)
	{
		var area = magnitude * _parameters.AreaCoefficient;
		return Mathf.Sqrt(area / Mathf.PI);
	}

	private float Compute_CoreRadius(float availableCircleRadius)
		=> Mathf.Max(availableCircleRadius, _parameters.MinCoreRadius);

	private float Compute_UnavailableRingWidth(float availableCircleRadius)
	{
		var unavailableWidth = _parameters.MinCoreRadius - availableCircleRadius;
		return unavailableWidth > 0 ? unavailableWidth : 0;
	}

	private float Compute_BorderWidth(float coreRadius)
		=> coreRadius * _parameters.BorderToCoreRatio;
}



}
