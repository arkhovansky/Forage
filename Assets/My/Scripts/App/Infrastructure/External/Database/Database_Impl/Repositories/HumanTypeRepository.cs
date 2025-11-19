using System.Collections.Generic;

using App.Game.Database;



namespace App.Infrastructure.External.Database.Database_Impl.Repositories {



public class HumanTypeRepository : IHumanTypeRepository
{
	private readonly Dictionary<uint, HumanType> _humanTypes = new();



	public HumanTypeRepository()
	{
		_humanTypes[(uint)Gender.Male] = new HumanType {
			Gender = Gender.Male
		};
		_humanTypes[(uint)Gender.Female] = new HumanType {
			Gender = Gender.Female
		};
	}


	public HumanType Get(uint typeId)
	{
		return _humanTypes[typeId];
	}
}



}
