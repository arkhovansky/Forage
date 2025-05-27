using Unity.Entities;
using UnityEngine;

using App.Game.ECS.Components;



namespace App.Game.ECS.Camp.Authoring {



public class Camp : MonoBehaviour
{
	private class Baker : Baker<Camp>
	{
		public override void Bake(Camp authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent<Components.Camp>(entity);
			AddComponent<TilePosition>(entity);
		}
	}
}



}
