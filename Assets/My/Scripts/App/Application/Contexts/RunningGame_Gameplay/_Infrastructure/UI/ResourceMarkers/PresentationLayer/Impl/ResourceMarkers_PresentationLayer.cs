using System;
using System.Collections.Generic;

using Lib.AppFlow;

using App.Game.Core.Query;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationLayer.Impl {



/// <summary>
/// Presentation layer managing all resource markers.
/// </summary>
public class ResourceMarkers_PresentationLayer
	: ILoopComponent
{
	private readonly IPlantResources _plantResources;

	private readonly ITime _time;

	private readonly IResourceMarker_Pool _marker_Pool;

	private readonly List<IResourceMarker> _markers = new();

	//----------------------------------------------------------------------------------------------


	public ResourceMarkers_PresentationLayer(
		IPlantResources plantResources,
		ITime time,
		IResourceMarker_Pool marker_Pool)
	{
		_plantResources = plantResources;
		_time = time;
		_marker_Pool = marker_Pool;
	}


	//----------------------------------------------------------------------------------------------
	// ILoopComponent


	public void LateUpdate()
	{
		if (_time.Get_YearPeriodChanged())
			UpdateChildrenList();

		UpdateChildren();
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void UpdateChildrenList()
	{
		var resources = _plantResources.Get_RipeResources();

		// Assume resources are single-season

		var minCount = Math.Min(resources.Count, _markers.Count);
		for (var i = 0; i < minCount; i++)
			_markers[i].Reset(resources[i]);

		if (_markers.Count < resources.Count ) {
			for (var i = _markers.Count; i < resources.Count; i++) {
				var marker = _marker_Pool.Acquire();
				marker.Reset(resources[i]);
				_markers.Add(marker);
			}
		}
		else {
			for (var i = resources.Count; i < _markers.Count; i++)
				_marker_Pool.Release(_markers[i]);
			_markers.RemoveRange(resources.Count, _markers.Count - resources.Count);
		}
	}


	private void UpdateChildren()
	{
		foreach (var marker in _markers)
			marker.LateUpdate();
	}
}



}
