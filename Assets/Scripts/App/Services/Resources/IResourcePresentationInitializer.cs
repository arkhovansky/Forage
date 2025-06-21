using System.Collections.Generic;



namespace App.Services.Resources {



public interface IResourcePresentationInitializer
{
	void Init(ISet<uint> resourceTypeIds);
}



}
