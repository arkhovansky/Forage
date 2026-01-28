using App.Game.Core.Query;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.ResourceMarkers.Shared {



public interface IResettableToResource
{
	/// <summary>
	/// Reset to the given resource.
	/// </summary>
	/// <param name="resource"></param>
	void Reset(IPlantResource resource);
}



}
