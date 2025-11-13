using App.Game.ECS.Resource.Plant.Components;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface IPlantResource
{
	PlantResource Get_StaticData();

	float Get_RipeBiomass();
}



}
