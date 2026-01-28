using UnityEngine;

using App.Infrastructure.Shared.Contracts.Database.Presentation.Types;



namespace App.Infrastructure.Shared.Contracts.Database.Presentation {



public interface IResourceMarker_Config_Repository
{
	GameObject Prefab { get; }

	ResourceMarker_Parameters Parameters { get; }
}



}
