using Unity.Entities;

using App.Game.ECS.Map;



namespace App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Services {



/// <summary>
/// Collection of ECS helpers.
/// </summary>
public interface IEcsHelper
{
	Entity GetSingletonEntity();

	void AddSingletonComponent<T>(T componentData)
		where T : unmanaged, IComponentData;

	void SendEcsCommand<T>(T command)
		where T : unmanaged, IComponentData;

	bool IsEventRaised<T>()
		where T : unmanaged, IComponentData;

	/// <summary>
	/// Checks if a singleton component of the specified type exists anywhere,
	/// i.e. not necessarily on the SingletonEntity
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	bool SingletonExistsAnywhere<T>()
		where T : unmanaged, IComponentData;

	EcsMap GetEcsMap();
}



}
