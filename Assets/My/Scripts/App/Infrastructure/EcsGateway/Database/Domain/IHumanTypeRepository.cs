using App.Game.Database;



namespace App.Infrastructure.EcsGateway.Database.Domain {



public interface IHumanTypeRepository
{
	HumanType Get(HumanTypeId typeId);
}



}
