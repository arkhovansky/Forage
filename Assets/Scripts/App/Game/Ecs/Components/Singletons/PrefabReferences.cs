using Unity.Entities;



namespace App.Game.Ecs.Components.Singletons {



public struct PrefabReferences : IComponentData
{
	public Entity Man;
	public Entity Woman;

	public Entity Camp;
}



}
