using Unity.Entities;

using App.Game.ECS.Map;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Util.Components;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Infrastructure.EcsGateway.Services {



public class EcsHelper : IEcsHelper
{
	private readonly Entity _singletonEntity
		= Do_GetSingletonEntity();


	//----------------------------------------------------------------------------------------------
	// IEcsHelper


	public Entity GetSingletonEntity()
		=> _singletonEntity;


	public void AddSingletonComponent<T>(T componentData)
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		entityManager.AddComponentData(_singletonEntity, componentData);
	}


	public T GetSingletonComponent<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.GetComponentData<T>(_singletonEntity);
	}


	public void SendEcsCommand<T>(T command)
		where T : unmanaged, IComponentData
	{
		AddSingletonComponent(command);
	}


	public bool IsEventRaised<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.HasComponent<T>(_singletonEntity);
	}


	public bool HasSingleton_Anywhere<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<T>());
		return query.HasSingleton<T>();
	}


	public T GetSingleton_Anywhere<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<T>());
		return query.GetSingleton<T>();
	}


	public EcsMap GetEcsMap()
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;

		var map = em.GetComponentData<Map>(_singletonEntity);
		var tileBuffer = em.GetBuffer<MapTileEntity>(_singletonEntity, isReadOnly: true);
		return new EcsMap(map, tileBuffer);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static Entity Do_GetSingletonEntity()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<SingletonEntity_Tag>());
		return query.GetSingletonEntity();
	}
}



}
