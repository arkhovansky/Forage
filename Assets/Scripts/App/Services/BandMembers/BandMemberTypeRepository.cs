using System.Collections.Generic;



namespace App.Services.BandMembers {



public class BandMemberTypeRepository : IBandMemberTypeRepository
{
	private readonly Dictionary<uint, BandMemberType> _bandMemberTypes = new();



	public BandMemberTypeRepository()
	{
		_bandMemberTypes[(uint)Gender.Male] = new BandMemberType {
			Gender = Gender.Male,
			GatheringSpeed = 1
		};
		_bandMemberTypes[(uint)Gender.Female] = new BandMemberType {
			Gender = Gender.Female,
			GatheringSpeed = 1
		};
	}


	public BandMemberType Get(uint typeId)
	{
		return _bandMemberTypes[typeId];
	}
}



}
