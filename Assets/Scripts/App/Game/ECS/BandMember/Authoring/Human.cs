using Unity.Entities;

using UnityEngine;



namespace App.Game.ECS.BandMember.Authoring {



public class Human : MonoBehaviour
{
	public uint TypeId;


	private class Baker : Baker<Human>
	{
		public override void Bake(Human authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new Components.Human {TypeId = authoring.TypeId});
			AddComponent<Components.BandMember>(entity);
		}
	}
}



}
