using System.Collections.Generic;

using App.Game.Database;
using App.Infrastructure.EcsGateway.Database.Domain;



namespace App.Infrastructure.External.Data.Database.Domain.Repositories {



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
