using Unity.Entities;



namespace App.Game.ECS.Util.Components {



/// <summary>
/// Singleton component for tagging the singleton entity.
/// </summary>
/// <remarks>
/// The singleton entity is used for all components which are inherently singletons: commands, events etc.
/// </remarks>
public struct SingletonEntity_Tag : IComponentData { }



}
