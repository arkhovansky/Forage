using Unity.Entities;

using UnityEngine;

using App.Game.ECS.BandMember.Gathering.Components;



namespace App.Game.ECS.BandMember.Gathering.Authoring {



public class Forager : MonoBehaviour
{
	public float GatheringSpeed;


	private class Baker : Baker<Forager>
	{
		public override void Bake(Forager authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);

			AddComponent(entity, new Components.Forager {GatheringSpeed = authoring.GatheringSpeed});

			AddComponent<GatheringActivity>(entity);
			SetComponentEnabled<GatheringActivity>(entity, false);
		}
	}
}



}
