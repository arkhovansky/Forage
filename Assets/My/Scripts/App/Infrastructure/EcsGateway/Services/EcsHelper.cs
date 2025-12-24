using Unity.Entities;

using App.Game.ECS.Map;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Util.Components;
using App.Infrastructure.EcsGateway.Contracts.Services;



namespace App.Infrastructure.EcsGateway.Services {



/// <remarks>
/// ECS uses single entity for all singleton components. It is tagged with SingletonEntity_Tag component.
/// </remarks>
public class EcsHelper : IEcsHelper
{
	public Entity GetSingletonEntity()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.CreateEntityQuery(ComponentType.ReadOnly<SingletonEntity_Tag>())
			.GetSingletonEntity();
	}


	public void AddSingletonComponent<T>(T componentData)
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, componentData);
	}


	public T GetSingletonComponent<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.GetComponentData<T>(GetSingletonEntity());
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
		var singletonEntity = GetSingletonEntity();

		return entityManager.HasComponent<T>(singletonEntity);
	}


	public bool SingletonExistsAnywhere<T>()
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.CreateEntityQuery(ComponentType.ReadOnly<T>()).HasSingleton<T>();
	}


	public EcsMap GetEcsMap()
	{
		var em = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = GetSingletonEntity();

		var map = em.GetComponentData<Map>(singletonEntity);
		var tileBuffer = em.GetBuffer<MapTileEntity>(singletonEntity, isReadOnly: true);
		return new EcsMap(map, tileBuffer);
	}
}



}
