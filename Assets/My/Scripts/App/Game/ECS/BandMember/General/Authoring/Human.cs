using Unity.Entities;
using UnityEngine;



namespace App.Game.ECS.BandMember.General.Authoring {



public class Human : MonoBehaviour
{
	private class Baker : Baker<Human>
	{
		public override void Bake(Human authoring)
		{
			GetEntity(TransformUsageFlags.Dynamic);
		}
	}
}



}
