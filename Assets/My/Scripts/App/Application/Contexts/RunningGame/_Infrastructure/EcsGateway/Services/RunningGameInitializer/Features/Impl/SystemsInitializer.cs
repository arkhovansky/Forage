using Unity.Entities;

using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Database.DomainSettings;
using App.Game.ECS.BandMember.Gathering.Systems;
using App.Game.ECS.BandMember.Movement.Systems;
using App.Game.ECS.GameTime.Systems;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services.RunningGameInitializer.Features.Impl {



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
