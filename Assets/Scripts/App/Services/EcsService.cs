using Unity.Entities;

using App.Game.ECS.Util.Components;



namespace App.Services {



/// <summary>
/// Static class with functions for sending singleton data to ECS world.
/// </summary>
/// <remarks>
/// ECS uses single entity for all singleton components. It is tagged with SingletonEntity_Tag component.
/// </remarks>
public static class EcsService
{
	public static void AddSingletonComponent<T>(T componentData)
		where T : unmanaged, IComponentData
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var singletonEntity = entityManager.CreateEntityQuery(ComponentType.ReadOnly<SingletonEntity_Tag>())
			.GetSingletonEntity();

		entityManager.AddComponentData(singletonEntity, componentData);
	}


	public static void SendEcsCommand<T>(T command)
		where T : unmanaged, IComponentData
	{
		AddSingletonComponent(command);
	}
}



}
