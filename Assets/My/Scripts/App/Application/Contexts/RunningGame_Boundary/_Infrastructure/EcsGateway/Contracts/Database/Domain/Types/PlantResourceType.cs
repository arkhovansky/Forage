using System;

using App.Game.Database;
using App.Game.ECS.GameTime.Components;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain {



[Serializable]
public class PlantResourceType
{
	public ResourceTypeId Id;

	public YearPeriod RipenessPeriod;
}



}
