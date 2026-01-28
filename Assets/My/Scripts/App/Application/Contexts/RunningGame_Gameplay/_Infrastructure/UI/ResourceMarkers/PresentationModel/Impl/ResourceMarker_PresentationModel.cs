using UniMob;
using AtomLifetime = UniMob.Lifetime;

using Lib.AppFlow;
using Lib.Grid;

using App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.Shared;
using App.Game.Core.Query;
using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.PresentationModel.Impl {



public class ResourceMarker_PresentationModel
	: IResourceMarker_PresentationModel,
	  IResettableToResource,
	  ILoopComponent
{
	private IPlantResource _resource = null!;

	private readonly MutableAtom<float> _magnitude_Atom;

	//----------------------------------------------------------------------------------------------


	public ResourceMarker_PresentationModel(AtomLifetime atomLifetime)
	{
		_magnitude_Atom = Atom.Value(atomLifetime, 0f, "ResourceMarker_PresentationModel: Magnitude");
	}


	//----------------------------------------------------------------------------------------------
	// IResettableToResource


	public void Reset(IPlantResource resource)
	{
		_resource = resource;

		ResourceType = _resource.Get_StaticData().TypeId;
		TilePosition = _resource.Get_Position();
	}


	//----------------------------------------------------------------------------------------------
	// IResourceMarker_PresentationModel


	public ResourceTypeId ResourceType { get; private set; }

	public AxialPosition TilePosition { get; private set; }


	public Atom<float> Magnitude_Atom
		=> _magnitude_Atom;


	//----------------------------------------------------------------------------------------------
	// ILoopComponent


	public void LateUpdate()
	{
		_magnitude_Atom.Value = _resource.Get_RipeBiomass();
	}
}



}
