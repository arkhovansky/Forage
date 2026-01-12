using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

using Lib.Grid;
using Lib.Grid.Spatial;

using App.Game.Database;
using App.Game.ECS.Map.Components;
using App.Game.ECS.Map.Components.Singletons;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Resource.Plant.Presentation.Components;
using App.Game.ECS.SystemGroups;

using Random = UnityEngine.Random;



namespace App.Game.ECS.Resource.Plant.Presentation.Systems {



[UpdateInGroup(typeof(LocalTransformPresentation))]
public partial struct PlantResourcePresentation : ISystem
{
	private struct ResourceIconsCreationData
	{
		public ResourceTypeId ResourceType;
		public AxialPosition MapPosition;
		public int IconCountToCreate;
		public Entity ResourceEntity;
	}

	//----------------------------------------------------------------------------------------------

	/// <summary>
	/// Icon side length relative to inner cell diameter.
	/// </summary>
	private const float RelativeIconSize = 0.2f;


	private NativeList<ResourceIconsCreationData> _creationData;

	private NativeList<Entity> _entitiesToDestroy;

	//----------------------------------------------------------------------------------------------


	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		_creationData = new NativeList<ResourceIconsCreationData>(Allocator.Persistent);
		_entitiesToDestroy = new NativeList<Entity>(Allocator.Persistent);
	}


	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		foreach (var (resource, position, ripeBiomass, icons, entity)
		         in SystemAPI.Query<
			         RefRO<PlantResource>, MapPosition, RipeBiomass, DynamicBuffer<ResourceIcon>>()
			         .WithEntityAccess())
		{
			const uint biomassPerIcon = 20;
			int neededIconCount = Mathf.CeilToInt(ripeBiomass.Value / biomassPerIcon);

			if (icons.Length == neededIconCount)
				continue;

			if (icons.Length < neededIconCount) {
				_creationData.Add(new ResourceIconsCreationData {
					ResourceType = resource.ValueRO.TypeId,
					MapPosition = position,
					IconCountToCreate = neededIconCount - icons.Length,
					ResourceEntity = entity
				});
			}
			else {
				for (var i = neededIconCount; i < icons.Length; i++)
					_entitiesToDestroy.Add(icons[i].Entity);

				var icons_ = icons;
				icons_.Length = neededIconCount;
			}
		}

		CreateIcons(_creationData,
		            ref state);
		DestroyIcons(_entitiesToDestroy,
		             ref state);

		_creationData.Clear();
		_entitiesToDestroy.Clear();
	}


	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
		_creationData.Dispose();
		_entitiesToDestroy.Dispose();
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void CreateIcons(NativeList<ResourceIconsCreationData> creationData,
	                         ref SystemState state)
	{
		var prototype = SystemAPI.GetComponent<ResourceIcon_Prototype>(state.SystemHandle).Entity;
		var mmiBuffer = SystemAPI.GetSingletonBuffer<ResourceIcon_MaterialMeshInfo>(true);
		var gridLayout = SystemAPI.GetSingleton<HexGridLayout_3D_Component>().Layout;

		var totalCount = CountIcons(creationData);

		using var clonedEntities = new NativeArray<Entity>(totalCount, Allocator.Temp);
		state.EntityManager.Instantiate(prototype, clonedEntities);

		for (int iResource = 0, iIcon = 0; iResource < creationData.Length; iResource++) {
			var resourceData = creationData[iResource];

			var materialMeshInfo = mmiBuffer[(int)resourceData.ResourceType].Value;

			var resourceIconsBuffer = state.EntityManager.GetBuffer<ResourceIcon>(resourceData.ResourceEntity);

			for (var iResourceIcon = 0; iResourceIcon < resourceData.IconCountToCreate; iResourceIcon++) {
				var entity = clonedEntities[iIcon++];
				uint iconIndexInResource = (uint) resourceIconsBuffer.Length;

				state.EntityManager.SetComponentData(entity,
					GetIconLocalTransform(resourceData.MapPosition, iconIndexInResource, gridLayout));

				state.EntityManager.SetComponentData(entity, materialMeshInfo);

#if !DOTS_DISABLE_DEBUG_NAMES
				SetIconName(entity, in resourceData, iconIndexInResource, ref state);
#endif

				resourceIconsBuffer.Add(new ResourceIcon(entity));
			}
		}
	}


	private void DestroyIcons(NativeList<Entity> iconEntities,
	                          ref SystemState state)
	{
		state.EntityManager.DestroyEntity(iconEntities.AsArray());
	}


	private int CountIcons(NativeList<ResourceIconsCreationData> creationData)
	{
		int count = 0;
		foreach (var data in creationData)
			count += data.IconCountToCreate;
		return count;
	}


	private LocalTransform GetIconLocalTransform(AxialPosition tilePosition, uint iconIndexInResource,
	                                             HexGridLayout_3D gridLayout)
	{
		var inTilePosition = GetIconInTilePosition(iconIndexInResource, gridLayout);
		return gridLayout.GetCellLocalTransform(tilePosition)
			.Translate(new float3(inTilePosition.x, 0.01f, inTilePosition.y))
			.ApplyScale(gridLayout.InnerCellRadius * 2 * RelativeIconSize);  // Assume icon mesh size is 1x1
	}

	// ReSharper disable once UnusedParameter.Local
	private Vector2 GetIconInTilePosition(uint iconIndexInResource,
	                                      HexGridLayout_3D gridLayout)
	{
		var iconRadius = gridLayout.InnerCellRadius * RelativeIconSize;
		var areaRadius = gridLayout.InnerCellRadius - iconRadius;
		return Random.insideUnitCircle * areaRadius;
	}


#if !DOTS_DISABLE_DEBUG_NAMES
	private void SetIconName(
		Entity entity,
		in ResourceIconsCreationData resourceData,
		uint iconIndexInResource,
		ref SystemState state)
	{
		FixedString64Bytes name = "Resource icon: type ";
		name.Append((int) resourceData.ResourceType);

		name.Append((FixedString32Bytes) " (");
		name.Append(resourceData.MapPosition.Q);
		name.Append((FixedString32Bytes) ", ");
		name.Append(resourceData.MapPosition.R);
		name.Append(')');

		name.Append((FixedString32Bytes) " - ");
		name.Append(iconIndexInResource);

		state.EntityManager.SetName(entity, name);
	}
#endif
}



}
