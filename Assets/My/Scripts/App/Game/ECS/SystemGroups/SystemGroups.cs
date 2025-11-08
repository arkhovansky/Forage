using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

using App.Infrastructure.ECS.Services;



namespace App.Game.ECS.SystemGroups {



[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class Simulation : ComponentSystemGroup {}


[UpdateInGroup(typeof(Simulation))]
public partial class DomainSimulation : ComponentSystemGroup {}


[UpdateInGroup(typeof(Simulation))]
[UpdateAfter(typeof(DomainSimulation))]
public partial class DiscreteActions : ComponentSystemGroup {}


[UpdateInGroup(typeof(Simulation))]
[UpdateAfter(typeof(DiscreteActions))]
public partial class HumanAI : ComponentSystemGroup {}



/// <summary>
/// For presentation systems that modify (or create entities with) LocalTransform
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial class LocalTransformPresentation : ComponentSystemGroup {}



/// <summary>
/// For presentation systems that make structural changes and do not modify LocalTransform
/// </summary>
[UpdateInGroup(typeof(StructuralChangePresentationSystemGroup))]
public partial class StructuralChangePresentation : ComponentSystemGroup {}



public static class GameSystems
{
	private static bool _enabled = true;


	public static bool Enabled {
		get => _enabled;
		set {
			if (value == _enabled)
				return;
			SetEnabled(value);
			_enabled = value;
		}
	}


	private static void SetEnabled(bool enabled)
	{
		EcsService.SetSystemGroupEnabled<Simulation>(enabled);
		EcsService.SetSystemGroupEnabled<LocalTransformPresentation>(enabled);
		EcsService.SetSystemGroupEnabled<StructuralChangePresentation>(enabled);
	}
}



}
