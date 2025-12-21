using App.Game.Database;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface IHumanTypeRepository
{
	HumanType Get(HumanTypeId typeId);
}



}
