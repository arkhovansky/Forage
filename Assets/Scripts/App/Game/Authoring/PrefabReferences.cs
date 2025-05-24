using Unity.Entities;
using UnityEngine;



namespace App.Game.Authoring {



public class PrefabReferences : MonoBehaviour
{
	public GameObject ManPrefab = null!;
	public GameObject WomanPrefab = null!;

	public GameObject CampPrefab = null!;


	public class Baker : Baker<PrefabReferences>
	{
		public override void Bake(PrefabReferences authoring)
		{
			DependsOn(authoring.ManPrefab);
			DependsOn(authoring.WomanPrefab);
			DependsOn(authoring.CampPrefab);

			if (authoring.ManPrefab == null)
				throw new System.NullReferenceException($"{nameof(PrefabReferences)}.{nameof(ManPrefab)} is null");
			if (authoring.WomanPrefab == null)
				throw new System.NullReferenceException($"{nameof(PrefabReferences)}.{nameof(WomanPrefab)} is null");
			if (authoring.CampPrefab == null)
				throw new System.NullReferenceException($"{nameof(PrefabReferences)}.{nameof(CampPrefab)} is null");

			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity,
				new Ecs.Components.Singletons.PrefabReferences {
					// TransformUsageFlags.None for entities having their own bakers
					Man = GetEntity(authoring.ManPrefab, TransformUsageFlags.None),
					Woman = GetEntity(authoring.WomanPrefab, TransformUsageFlags.None),
					Camp = GetEntity(authoring.CampPrefab, TransformUsageFlags.None)
				});
		}
	}
}



}
