using App.Game.ECS.Resource.Plant.Components;



namespace App.Game.Core.Query {



public interface IPlantResource
{
	PlantResource Get_StaticData();

	float Get_RipeBiomass();
}



}
