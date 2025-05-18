using System.Collections.Generic;



namespace App.Services.BandMembers {



public interface IBandInitializer
{
	void Init(IDictionary<uint, uint> bandMemberTypeCounts);
}



}
