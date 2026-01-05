using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain {



public enum Gender
{
	Male,
	Female
}



/// <summary>
/// A human type
/// </summary>
/// <param name="Id"></param>
/// <param name="Gender"></param>
/// <param name="EnergyRequiredDaily">Energy required daily, kcal</param>
/// <param name="BaseSpeed">Base movement speed, km/h</param>
/// <param name="GatheringSpeed">Gathering speed, kg/h</param>
public record HumanType(
	HumanTypeId Id,
	Gender Gender,
	uint EnergyRequiredDaily,
	float BaseSpeed,
	float GatheringSpeed
);



}
