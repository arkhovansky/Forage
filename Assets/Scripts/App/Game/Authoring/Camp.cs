using Unity.Entities;
using UnityEngine;

using App.Game.Ecs.Components;



namespace App.Game.Authoring {



public class Camp : MonoBehaviour
{
	private class Baker : Baker<Camp>
	{
		public override void Bake(Camp authoring)
		{
			var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
			AddComponent<TilePosition>(entity);
		}
	}
}



}
