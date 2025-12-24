using System.Collections.Generic;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.Repositories {



public class HumanTypeRepository : IHumanTypeRepository
{
	private readonly Dictionary<HumanTypeId, HumanType> _humanTypes = new();



	public HumanTypeRepository()
	{
		_humanTypes[HumanTypeId.Man] = new HumanType {
			Gender = Gender.Male
		};
		_humanTypes[HumanTypeId.Woman] = new HumanType {
			Gender = Gender.Female
		};
	}


	public HumanType Get(HumanTypeId typeId)
	{
		return _humanTypes[typeId];
	}
}



}
