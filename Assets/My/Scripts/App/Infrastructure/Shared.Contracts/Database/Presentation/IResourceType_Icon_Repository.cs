using UnityEngine;

using App.Game.Database;



namespace App.Infrastructure.Shared.Contracts.Database.Presentation {



public interface IResourceType_Icon_Repository
{
	Texture2D Get(ResourceTypeId typeId);
}



}
