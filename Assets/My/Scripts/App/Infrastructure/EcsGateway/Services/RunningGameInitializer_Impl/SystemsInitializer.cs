using Unity.Entities;

using App.Game.Database;
using App.Game.ECS.BandMember.Gathering.Systems;
using App.Game.ECS.BandMember.Movement.Systems;
using App.Game.ECS.GameTime.Systems;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public class SystemsInitializer : ISystemsInitializer
{
	private readonly ISystemParametersRepository _repository;



	public SystemsInitializer(ISystemParametersRepository repository)
	{
		_repository = repository;
	}


	public void Init()
	{
		var world = World.DefaultGameObjectInjectionWorld;

		var system = world.GetExistingSystem<GameTimeSystem>();
		world.EntityManager.AddComponentData(system, _repository.Get_GameTime_Rules());

		system = world.GetExistingSystem<DaylightSystem>();
		world.EntityManager.AddComponentData(system, _repository.Get_Daylight_Rules());

		system = world.GetExistingSystem<Movement_System>();
		world.EntityManager.AddComponentData(system, _repository.Get_Movement_Rules());

		system = world.GetExistingSystem<Gathering_System>();
		world.EntityManager.AddComponentData(system, _repository.Get_Gathering_Rules());
	}
}



}
