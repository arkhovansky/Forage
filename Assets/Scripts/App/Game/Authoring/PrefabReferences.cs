using Unity.Entities;
using UnityEngine;



namespace App.Game.Authoring {



public class PrefabReferences : MonoBehaviour
{
	public GameObject CampPrefab;


	public class Baker : Baker<PrefabReferences>
	{
		public override void Bake(PrefabReferences authoring)
		{
			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity,
				new Ecs.Components.Singletons.PrefabReferences {
					Camp = GetEntity(authoring.CampPrefab, TransformUsageFlags.Dynamic)
				});
		}
	}
}



}
