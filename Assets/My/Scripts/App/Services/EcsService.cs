using Unity.Entities;

using App.Game.ECS.Util.Components;



namespace App.Services {



/// <summary>
/// Collection of ECS helpers.
/// </summary>
/// <remarks>
/// ECS uses single entity for all singleton components. It is tagged with SingletonEntity_Tag component.
/// </remarks>
public static class EcsService
{
	public static void SetSystemGroupEnabled<T>(bool enabled) where T : ComponentSystemGroup
	{
		var world = World.DefaultGameObjectInjectionWorld;
		world.GetExistingSystemManaged<T>().Enabled = enabled;
	}


	public static void SetEcsSystemsEnabled(bool enabled)
	{
		SetSystemGroupEnabled<InitializationSystemGroup>(enabled);
		SetSystemGroupEnabled<SimulationSystemGroup>(enabled);
		SetSystemGroupEnabled<PresentationSystemGroup>(enabled);
	}


	public static Entity GetSingletonEntity()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.CreateEntityQuery(ComponentType.ReadOnly<SingletonEntity_Tag>())
			.GetSingletonEntity();
	}


	public static void AddSingletonComponent<T>(T componentData)
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, componentData);
	}


	public static void SendEcsCommand<T>(T command)
		where T : unmanaged, IComponentData
	{
		AddSingletonComponent(command);
	}


	public static bool IsEventRaised<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = GetSingletonEntity();

		return entityManager.HasComponent<T>(singletonEntity);
	}
}



}
