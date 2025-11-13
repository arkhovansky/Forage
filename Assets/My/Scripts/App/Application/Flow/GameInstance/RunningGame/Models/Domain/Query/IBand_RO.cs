using System.Collections.Generic;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface IBand_RO
{
	IReadOnlyList<IBandMember_RO> Get_Members();
}



}
