using Unity.Entities;



namespace App.Game.ECS.Prefabs.Components {



public struct PrefabReferences : IComponentData
{
	public Entity Man;
	public Entity Woman;

	public Entity Camp;
}



}
