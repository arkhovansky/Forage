using UnityEngine;

using UniMob;
using AtomLifetime = UniMob.Lifetime;

using Lib.AppFlow;
using Lib.Grid.Spatial;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationLayer.Impl;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.RenderModel.Impl;
using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.View.Impl;
using App.Game.Core.Query;
using App.Infrastructure.Shared.Contracts.Database.Presentation;




namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers {



public static class ResourceMarkers_PresentationLayer_Composer
{
	public static ILoopComponent Compose(
		IPlantResources plantResources, ITime time,
		in HexGridLayout_3D gridLayout,
		IResourceMarker_Config_Repository resourceMarker_Config_Repository,
		IResourceType_Icon_Repository resourceType_Icon_Repository,
		Camera camera,
		Atom<Transform> cameraTransform_Atom,
		AtomLifetime atomLifetime)
	{
		var resourceMarker_RenderModel_Factory =
			new ResourceMarker_RenderModel_Factory(
				resourceMarker_Config_Repository.Parameters,
				in gridLayout);
		var resourceMarker_View_Pool =
			new ResourceMarker_View_Pool(
				resourceMarker_Config_Repository.Prefab,
				resourceType_Icon_Repository,
				camera,
				cameraTransform_Atom);
		var resourceMarker_Pool =
			new ResourceMarker_Pool(
				resourceMarker_RenderModel_Factory,
				resourceMarker_View_Pool,
				atomLifetime);
		return
			new ResourceMarkers_PresentationLayer(
				plantResources, time,
				resourceMarker_Pool);
	}
}



}
