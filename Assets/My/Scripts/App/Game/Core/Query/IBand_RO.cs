using System.Collections.Generic;



namespace App.Game.Core.Query {



public interface IBand_RO
{
	IReadOnlyList<IBandMember_RO> Get_Members();
}



}
