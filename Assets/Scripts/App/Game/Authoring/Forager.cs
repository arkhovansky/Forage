using Unity.Entities;

using UnityEngine;



namespace App.Game.Authoring {



public class Forager : MonoBehaviour
{
	public float GatheringSpeed;


	private class Baker : Baker<Forager>
	{
		public override void Bake(Forager authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new Ecs.Components.BandMember.Forager {GatheringSpeed = authoring.GatheringSpeed});
		}
	}
}



}
