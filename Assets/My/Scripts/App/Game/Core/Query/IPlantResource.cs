using Lib.Grid;

using App.Game.ECS.Resource.Plant.Components;



namespace App.Game.Core.Query {



public interface IPlantResource
{
	AxialPosition Get_Position();

	PlantResource Get_StaticData();

	float Get_RipeBiomass();
}



}
