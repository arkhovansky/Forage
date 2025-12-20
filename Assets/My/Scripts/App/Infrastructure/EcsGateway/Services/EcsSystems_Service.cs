using Unity.Entities;

using App.Game.ECS.SystemGroups;
using App.Infrastructure.Common.Contracts.Services;



namespace App.Infrastructure.EcsGateway.Services {



public class EcsSystems_Service : IEcsSystems_Service
{
	private static bool _gameSystems_Enabled = true;


	//----------------------------------------------------------------------------------------------
	// IEcsSystems_Service implementation


	public void SetEcsSystemsEnabled(bool enabled)
	{
		SetSystemGroupEnabled<InitializationSystemGroup>(enabled);
		SetSystemGroupEnabled<SimulationSystemGroup>(enabled);
		SetSystemGroupEnabled<PresentationSystemGroup>(enabled);
	}


	public bool GameSystems_Enabled {
		get => _gameSystems_Enabled;
		set {
			if (value == _gameSystems_Enabled)
				return;
			Set_RootGameSystemGroups_Enabled(value);
			_gameSystems_Enabled = value;
		}
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
