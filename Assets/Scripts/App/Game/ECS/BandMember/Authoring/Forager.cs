using Unity.Entities;

using UnityEngine;



namespace App.Game.ECS.BandMember.Authoring {



public class Forager : MonoBehaviour
{
	public float GatheringSpeed;


	private class Baker : Baker<Forager>
	{
		public override void Bake(Forager authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new Components.Forager {GatheringSpeed = authoring.GatheringSpeed});
		}
	}
}



}
