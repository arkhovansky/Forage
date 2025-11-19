using App.Game.Database;
using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Statistics.Components;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface IBandMember_RO
{
	int Id { get; }

	HumanTypeId TypeId { get; }


	Goal? Get_Goal();


	enum ActivityType
	{
		Leisure,
		Sleeping,
		Moving,
		Gathering
	}

	ActivityType? Get_Activity();


	YearPeriodStatistics Get_YearPeriodStatistics();
}



}
