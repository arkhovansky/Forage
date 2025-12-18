using Unity.Entities;

using App.Game.ECS.Map;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.Util.Components;



namespace App.Infrastructure.EcsGateway.Services {



/// <summary>
/// Collection of ECS helpers.
/// </summary>
/// <remarks>
/// ECS uses single entity for all singleton components. It is tagged with SingletonEntity_Tag component.
/// </remarks>
public static class EcsService
{
	private static bool _gameSystems_Enabled = true;



	public static void SetEcsSystemsEnabled(bool enabled)
	{
		SetSystemGroupEnabled<InitializationSystemGroup>(enabled);
		SetSystemGroupEnabled<SimulationSystemGroup>(enabled);
		SetSystemGroupEnabled<PresentationSystemGroup>(enabled);
	}


	public static bool GameSystems_Enabled {
		get => _gameSystems_Enabled;
		set {
			if (value == _gameSystems_Enabled)
				return;
			Set_RootGameSystemGroups_Enabled(value);
			_gameSystems_Enabled = value;
		}
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


	/// <summary>
	/// Checks if a singleton component of the specified type exists anywhere,
	/// i.e. not necessarily on the SingletonEntity
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static bool SingletonExistsAnywhere<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.CreateEntityQuery(ComponentType.ReadOnly<T>()).HasSingleton<T>();
	}


	public static EcsMap GetEcsMap()
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = GetSingletonEntity();

		var map = em.GetComponentData<Map>(singletonEntity);
		var tileBuffer = em.GetBuffer<MapTileEntity>(singletonEntity, isReadOnly: true);
		return new EcsMap(map, tileBuffer);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static void SetSystemGroupEnabled<T>(bool enabled) where T : ComponentSystemGroup
	{
		var world = World.DefaultGameObjectInjectionWorld;
		world.GetExistingSystemManaged<T>().Enabled = enabled;
	}


	/// <summary>
	/// Set Enabled state of root custom system groups
	/// </summary>
	/// <param name="enabled"></param>
	private static void Set_RootGameSystemGroups_Enabled(bool enabled)
	{
		SetSystemGroupEnabled<Simulation>(enabled);
		SetSystemGroupEnabled<LocalTransformPresentation>(enabled);
		SetSystemGroupEnabled<StructuralChangePresentation>(enabled);
	}
}



}
