using Unity.Entities;

using UnityEngine;



namespace App.Game.ECS.BandMember.Energy.Authoring {



public class FoodConsumer : MonoBehaviour
{
	public uint NeededEnergyPerDay;


	private class Baker : Baker<FoodConsumer>
	{
		public override void Bake(FoodConsumer authoring)
		{
			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity, new Components.FoodConsumer(authoring.NeededEnergyPerDay));
		}
	}
}



}
