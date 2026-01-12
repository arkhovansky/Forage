using Unity.Entities;

using Lib.Grid.Spatial;



namespace App.Game.ECS.Map.Components.Singletons {



public struct HexGridLayout_3D_Component : IComponentData
{
	public readonly HexGridLayout_3D Layout;


	public HexGridLayout_3D_Component(HexGridLayout_3D layout)
	{
		Layout = layout;
	}
}



}
