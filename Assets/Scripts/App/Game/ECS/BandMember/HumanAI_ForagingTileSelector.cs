// ReSharper disable once RedundantNullableDirective
#nullable enable  // Suppress warning CS8669

using System.Collections.Generic;

using Unity.Entities;

using Lib.Grid;
using Lib.VisualGrid;

using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Components;
using App.Game.ECS.SystemGroups;



namespace App.Game.ECS.BandMember {



[UpdateInGroup(typeof(HumanAI))]
[UpdateAfter(typeof(HumanAI_GoalSelector))]
public partial class HumanAI_ForagingTileSelector : SystemBase
{
	private record PathInfo(
		IReadOnlyList<AxialPosition> Path,
		float Cost
	);

	private record TargetResource(
		AxialPosition Position,
		PathInfo PathInfo
	);


	// private RectangularHexMap? _map;



	// public void InitForScene(RectangularHexMap map)
	// {
	// 	_map = map;
	// }



	protected override void OnUpdate()
	{
		// if (_map == null)
		// 	return;

		foreach (var (foragerPosition, path, foragerEntity) in
		         SystemAPI.Query<RefRO<TilePosition>, DynamicBuffer<PathTile>>()
			         .WithAll<Foraging>()
			         .WithDisabled<TargetTile>()
			         .WithEntityAccess())
		{
			TargetResource? target = null;

			foreach (var resourcePosition in
			         SystemAPI.Query<RefRO<TilePosition>>())
			{
				var pathInfo = CalculatePath(foragerPosition.ValueRO.Position, resourcePosition.ValueRO.Position);
				if (target == null || pathInfo.Cost < target.PathInfo.Cost) {
					target = new TargetResource(resourcePosition.ValueRO.Position, pathInfo);
				}
			}

			if (target == null)
				continue;

			SystemAPI.SetComponent(foragerEntity, new TargetTile(target.Position));
			SystemAPI.SetComponentEnabled<TargetTile>(foragerEntity, true);

			var path_ = path;
			path_.Length = 0;
			foreach (var position in target.PathInfo.Path)
				path_.Add(new PathTile(position));
		}
	}



	private PathInfo CalculatePath(AxialPosition start, AxialPosition end)
	{
		var path = HexLayout.GetLinearPath(start, end);

		return new PathInfo(path, path.Length * 1f);
	}
}



}
