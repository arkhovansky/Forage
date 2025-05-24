using Unity.Entities;

using UnityEngine;



namespace App.Game.Authoring {



public class Human : MonoBehaviour
{
	public uint TypeId;


	private class Baker : Baker<Human>
	{
		public override void Bake(Human authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new Ecs.Components.BandMember.Human {TypeId = authoring.TypeId});
			AddComponent<Ecs.Components.BandMember.BandMember>(entity);
		}
	}
}



}
