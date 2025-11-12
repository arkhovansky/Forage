using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;



namespace App.Game.ECS.SystemGroups {


/***************************************************************************************************
* NB!
* All root custom groups must be also present in EcsService in EcsGateway layer
***************************************************************************************************/



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



}
