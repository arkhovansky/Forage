using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Contracts.Database.Domain {



public interface IHumanTypeRepository
{
	HumanType Get(HumanTypeId typeId);
}



}
