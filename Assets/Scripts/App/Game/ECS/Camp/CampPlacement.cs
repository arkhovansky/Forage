using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.Camp.Components.Commands;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Prefabs.Components;
using App.Game.ECS.SystemGroups;
using App.Game.ECS.Util.Components;



namespace App.Game.ECS.Camp {



[UpdateInGroup(typeof(DiscreteActions))]
public partial struct CampPlacement : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PlaceCamp>();
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var singletonEntity = SystemAPI.GetSingletonEntity<SingletonEntity_Tag>();

		var placeCampCommand = SystemAPI.GetSingleton<PlaceCamp>();

		PlaceCamp(placeCampCommand.Position, ref state);
		MoveBandMembersToCamp(placeCampCommand.Position, ref state);
		state.EntityManager.AddComponent<GameTimeRun>(singletonEntity);

		state.EntityManager.RemoveComponent<PlaceCamp>(singletonEntity);
	}


	private void PlaceCamp(AxialPosition position, ref SystemState state)
	{
		var prefabReferences = SystemAPI.GetSingleton<PrefabReferences>();

		var campEntity = state.EntityManager.Instantiate(prefabReferences.Camp);

		state.EntityManager.SetComponentData(campEntity, new TilePosition(position));

		var hexLayout = SystemAPI.GetSingleton<HexLayout3D_Component>().Layout;
		var inTilePosition = GetCampInTilePosition(in hexLayout);
		var localTransform = hexLayout.GetCellLocalTransform(position)
			.Translate(new float3(inTilePosition.x, 0.01f, inTilePosition.y))
			.ApplyScale(0.25f);
		state.EntityManager.SetComponentData(campEntity, localTransform);
	}


	private void MoveBandMembersToCamp(AxialPosition campPosition, ref SystemState state)
	{
		foreach (var (position, localTransform) in
		         SystemAPI.Query<RefRW<TilePosition>, RefRW<LocalTransform>>()
			         .WithAll<BandMember.Components.BandMember>())
		{
			position.ValueRW.Position = campPosition;

			var hexLayout = SystemAPI.GetSingleton<HexLayout3D_Component>().Layout;
			var inTilePosition = GetCampInTilePosition(in hexLayout);
			localTransform.ValueRW = hexLayout.GetCellLocalTransform(campPosition)
				.Translate(new float3(inTilePosition.x, 0.005f, inTilePosition.y));
		}
	}


	private Vector2 GetCampInTilePosition(in HexLayout3D hexLayout)
	{
		return new Vector2(0, 0);
	}
}



}
