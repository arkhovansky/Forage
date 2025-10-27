using Unity.Entities;
using UnityEngine;

using App.Game.ECS.BandMember.Movement.Components;



namespace App.Game.ECS.BandMember.Movement.Authoring {



public class Movement : MonoBehaviour
{
	public float BaseSpeed_KmPerH;


	private class Baker : Baker<Movement>
	{
		public override void Bake(Movement authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);

			AddComponent(entity, new Walker(authoring.BaseSpeed_KmPerH));

			AddBuffer<PathTile>(entity);

			AddComponent<IntraCellMovement>(entity);

			AddComponent<MovementActivity>(entity);
			SetComponentEnabled<MovementActivity>(entity, false);
		}
	}
}



}
