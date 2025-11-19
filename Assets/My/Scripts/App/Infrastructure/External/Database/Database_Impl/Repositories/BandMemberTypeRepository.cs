using System.Collections.Generic;

using App.Game.Database;



namespace App.Infrastructure.External.Database.Database_Impl.Repositories {



public class BandMemberTypeRepository : IBandMemberTypeRepository
{
	private readonly Dictionary<uint, BandMemberType> _bandMemberTypes = new();



	public BandMemberTypeRepository()
	{
		_bandMemberTypes[(uint)Gender.Male] = new BandMemberType {
			Gender = Gender.Male
		};
		_bandMemberTypes[(uint)Gender.Female] = new BandMemberType {
			Gender = Gender.Female
		};
	}


	public BandMemberType Get(uint typeId)
	{
		return _bandMemberTypes[typeId];
	}
}



}
