using Unity.Entities;

using UnityEngine;

using App.Game.ECS.BandMember.AI.Components;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.BandMember.Statistics.Components;
using App.Game.ECS.Map.Components;



namespace App.Game.ECS.BandMember.Authoring {



public class Human : MonoBehaviour
{
	public uint TypeId;


	private class Baker : Baker<Human>
	{
		public override void Bake(Human authoring)
		{
			var entity = GetEntity(TransformUsageFlags.Dynamic);

			AddComponent(entity, new Components.Human {TypeId = authoring.TypeId});
			AddComponent<Components.BandMember>(entity);
			AddComponent<MapPosition>(entity);

			AddComponent<GoalComponent>(entity);
			SetComponentEnabled<GoalComponent>(entity, false);

			AddComponent<Forage_Goal>(entity);
			SetComponentEnabled<Forage_Goal>(entity, false);

			AddComponent<Task>(entity);
			SetComponentEnabled<Task>(entity, false);

			AddComponent<ForageOnTile_Task>(entity);
			SetComponentEnabled<ForageOnTile_Task>(entity, false);

			AddComponent<Activity>(entity);
			SetComponentEnabled<Activity>(entity, false);

			AddComponent<Leisure_Goal>(entity);
			SetComponentEnabled<Leisure_Goal>(entity, false);

			AddComponent<Leisure_Task>(entity);
			SetComponentEnabled<Leisure_Task>(entity, false);

			AddComponent<LeisureActivity>(entity);
			SetComponentEnabled<LeisureActivity>(entity, false);

			AddComponent<Sleep_Goal>(entity);
			SetComponentEnabled<Sleep_Goal>(entity, false);

			AddComponent<Sleep_Task>(entity);
			SetComponentEnabled<Sleep_Task>(entity, false);

			AddComponent<SleepingActivity>(entity);
			SetComponentEnabled<SleepingActivity>(entity, false);

			AddComponent<DayStatistics>(entity);
			AddComponent<YearPeriodStatistics>(entity);
		}
	}
}



}
