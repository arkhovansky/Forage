using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface IHumanTypeRepository
{
	HumanType Get(HumanTypeId typeId);
}



}
