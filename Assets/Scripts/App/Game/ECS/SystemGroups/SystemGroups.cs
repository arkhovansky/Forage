using Unity.Entities;



namespace App.Game.ECS.SystemGroups {



[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial class DomainSimulation : ComponentSystemGroup
{
}



[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(DomainSimulation))]
public partial class DiscreteActions : ComponentSystemGroup
{
}



[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(DiscreteActions))]
public partial class HumanAI : ComponentSystemGroup
{
}



}
