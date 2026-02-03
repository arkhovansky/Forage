using Unity.Entities;

using App.Game.ECS.Map;



namespace App.Infrastructure.EcsGateway.Contracts.Services {



/// <summary>
/// Collection of helper methods for interacting with the ECS world.
/// </summary>
/// <remarks>
/// This contract assumes the ECS world contains a dedicated Singleton Entity tagged with the SingletonEntity_Tag
/// component, which most singleton components (including commands and events) are attached to.
/// </remarks>
public interface IEcsHelper
{
	/// <summary>
	/// Get the Singleton Entity.
	/// </summary>
	/// <returns></returns>
	Entity GetSingletonEntity();

	/// <summary>
	/// Add the provided singleton component to the Singleton Entity.
	/// </summary>
	/// <param name="componentData"></param>
	/// <typeparam name="T"></typeparam>
	void AddSingletonComponent<T>(T componentData)
		where T : unmanaged, IComponentData;

	/// <summary>
	/// Get the singleton component of the provided type from the Singleton Entity.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	T GetSingletonComponent<T>()
		where T : unmanaged, IComponentData;

	/// <summary>
	/// Put the provided command into the ECS world.
	/// </summary>
	/// <param name="command"></param>
	/// <typeparam name="T"></typeparam>
	void SendEcsCommand<T>(T command)
		where T : unmanaged, IComponentData;

	/// <summary>
	/// Check if an event of the provided type is raised.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	bool IsEventRaised<T>()
		where T : unmanaged, IComponentData;

	/// <summary>
	/// Check if the singleton component of the provided type exists anywhere,
	/// i.e., not necessarily on the Singleton Entity.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	bool HasSingleton_Anywhere<T>()
		where T : unmanaged, IComponentData;

	/// <summary>
	/// Get the singleton component of the provided type from anywhere,
	/// i.e., not necessarily from the Singleton Entity.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T GetSingleton_Anywhere<T>()
		where T : unmanaged, IComponentData;

	/// <summary>
	/// Get <see cref="EcsMap"/>.
	/// </summary>
	/// <returns></returns>
	EcsMap GetEcsMap();
}



}
