using Unity.Entities;

using UnityEngine;

using App.Game.ECS.BandMember.Gathering.Components;



namespace App.Game.ECS.BandMember.Gathering.Authoring {



public class Gatherer : MonoBehaviour
{
	public float GatheringSpeed;


	private class Baker : Baker<Gatherer>
	{
		public override void Bake(Gatherer authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);

			AddComponent(entity, new Components.Gatherer {GatheringSpeed = authoring.GatheringSpeed});

			AddComponent<GatheringActivity>(entity);
			SetComponentEnabled<GatheringActivity>(entity, false);
		}
	}
}



}
