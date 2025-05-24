using Unity.Entities;
using UnityEngine;



namespace App.Game.Authoring {



public class PrefabReferences : MonoBehaviour
{
	public GameObject CampPrefab = null!;


	public class Baker : Baker<PrefabReferences>
	{
		public override void Bake(PrefabReferences authoring)
		{
			DependsOn(authoring.CampPrefab);

			if (authoring.CampPrefab == null)
				throw new System.NullReferenceException($"{nameof(PrefabReferences)}.{nameof(CampPrefab)} is null");

			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity,
				new Ecs.Components.Singletons.PrefabReferences {
					// TransformUsageFlags.None for entities having their own bakers
					Camp = GetEntity(authoring.CampPrefab, TransformUsageFlags.None)
				});
		}
	}
}



}
