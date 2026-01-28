using System.Collections.Generic;



namespace App.Game.Core.Query {



public interface IPlantResources
{
	IReadOnlyList<IPlantResource> Get_RipeResources();
}



}
