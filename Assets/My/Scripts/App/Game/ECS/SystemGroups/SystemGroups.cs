using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;



namespace App.Game.ECS.SystemGroups {


/***************************************************************************************************
* NB!
* All root custom groups must be also present in EcsSystems_Service in EcsGateway layer
***************************************************************************************************/



[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class Simulation : ComponentSystemGroup {}


[UpdateInGroup(typeof(Simulation))]
[UpdateBefore(typeof(DomainSimulation))]
public partial class HumanAI : ComponentSystemGroup {}


[UpdateInGroup(typeof(Simulation))]
public partial class DomainSimulation : ComponentSystemGroup {}


[UpdateInGroup(typeof(Simulation))]
[UpdateAfter(typeof(DomainSimulation))]
public partial class DiscreteActions : ComponentSystemGroup {}



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
