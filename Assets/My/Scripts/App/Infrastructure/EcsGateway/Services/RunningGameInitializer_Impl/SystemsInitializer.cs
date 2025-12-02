using Unity.Entities;

using App.Game.Database;
using App.Game.ECS.BandMember.Gathering.Systems;
using App.Game.ECS.BandMember.Movement.Systems;
using App.Game.ECS.GameTime.Systems;
using App.Infrastructure.EcsGateway.Database.DomainSettings;



namespace App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl {



public class SystemsInitializer : ISystemsInitializer
{
	private readonly ISystemParametersRepository _rulesRepo;

	private readonly IDomainSettingsRepository _settingsRepo;



	public SystemsInitializer(ISystemParametersRepository rulesRepo,
	                          IDomainSettingsRepository settingsRepo)
	{
		_rulesRepo = rulesRepo;
		_settingsRepo = settingsRepo;
	}


	public void Init()
	{
		var world = World.DefaultGameObjectInjectionWorld;

		var system = world.GetExistingSystem<GameTimeSystem>();
		world.EntityManager.AddComponentData(system, _rulesRepo.Get_GameTime_Rules());
		world.EntityManager.AddComponentData(system, _settingsRepo.Get_GameTime_Settings());

		system = world.GetExistingSystem<DaylightSystem>();
		world.EntityManager.AddComponentData(system, _rulesRepo.Get_Daylight_Rules());

		system = world.GetExistingSystem<Movement_System>();
		world.EntityManager.AddComponentData(system, _rulesRepo.Get_Movement_Rules());

		system = world.GetExistingSystem<Gathering_System>();
		world.EntityManager.AddComponentData(system, _rulesRepo.Get_Gathering_Rules());
	}
}



}
